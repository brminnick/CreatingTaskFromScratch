namespace TooManyCooks.Common;

public abstract class Food
{
	readonly TimeSpan _cookTime;

	protected Food(TimeSpan cookTime)
	{
		_cookTime = cookTime;
		Name = GetType().Name;
	}
	
	public string Name { get; }
	
	public CustomTask.Task Cook() => CustomTask.Task.Delay(_cookTime); 
}

public class Turkey() : Food(TimeSpan.FromSeconds(10));
public class MashedPotatoes() : Food(TimeSpan.FromSeconds(2));
public class Gravy() : Food(TimeSpan.FromSeconds(1));
public class Stuffing() : Food(TimeSpan.FromSeconds(2));