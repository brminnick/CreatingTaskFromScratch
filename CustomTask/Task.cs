using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace CustomTask;

public class Task
{
	readonly Lock _lock = new();

	bool _completed;
	Exception? _exception;
	Action? _continuation;
	ExecutionContext? _context;

	public bool IsCompleted
	{
		get
		{
			lock (_lock)
			{
				return _completed;
			}
		}
	}

	public static Task Delay(TimeSpan delay)
	{
		Task task = new();

		new Timer(_ => task.SetResult()).Change(delay, Timeout.InfiniteTimeSpan);

		return task;
	}

	public static Task Run(Action action)
	{
		Task task = new();

		ThreadPool.QueueUserWorkItem(_ =>
		{
			try
			{
				action();
				task.SetResult();
			}
			catch (Exception e)
			{
				task.SetException(e);
			}
		});

		return task;
	}

	public void SetResult() => Complete(null);

	public void SetException(Exception exception) => Complete(exception);

	public void Wait()
	{
		ManualResetEventSlim? resetEvent = null;

		lock (_lock)
		{
			if (!_completed)
			{
				resetEvent = new();
				ContinueWith(resetEvent.Set);
			}
		}

		resetEvent?.Wait();

		if (_exception is not null)
		{
			ExceptionDispatchInfo.Throw(_exception);
		}
	}

	public Task ContinueWith(Action action)
	{
		Task task = new();

		lock (_lock)
		{
			if (_completed)
				ThreadPool.QueueUserWorkItem((object? state) =>
				{
					try
					{
						action();
						task.SetResult();
					}
					catch (Exception e)
					{
						task.SetException(e);
					}
				});
			else
			{
				_continuation = action;
				_context = ExecutionContext.Capture();
			}
		}

		return task;
	}

	public TaskAwaiter GetAwaiter() => new(this);

	void Complete(Exception? exception)
	{
		lock (_lock)
		{
			if (_completed)
				throw new InvalidOperationException("Cannot complete a completed task.");

			_completed = true;
			_exception = exception;

			if (_continuation is not null)
			{
				ThreadPool.QueueUserWorkItem(callBack =>
				{
					if (_context is null)
						_continuation?.Invoke();
					else
					{
						ExecutionContext.Run(_context, (object? state) => ((Action?)state)?.Invoke(), _continuation);
					}
				});
			}

			_continuation?.Invoke();
		}
	}
}

public readonly struct TaskAwaiter : INotifyCompletion
{
	readonly Task _task;

	internal TaskAwaiter(Task task) => _task = task;
	
	public bool IsCompleted => _task.IsCompleted;

	public void OnCompleted(Action continuation) => _task.ContinueWith(continuation);

	public TaskAwaiter GetAwaiter() => this;

	public void GetResult() => _task.Wait();
}