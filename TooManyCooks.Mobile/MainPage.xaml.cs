namespace TooManyCooks.Mobile;

public partial class MainPage : ContentPage
{
	int _count = 0;

	public MainPage()
	{
		InitializeComponent();
	}

	async void OnCounterClicked(object sender, EventArgs e)
	{
		_count++;

		await CustomTask.Task.Run(() => CustomTask.Task.Delay(TimeSpan.FromSeconds(2)));

		if (_count == 1)
			CounterBtn.Text = $"Clicked {_count} time";
		else
			CounterBtn.Text = $"Clicked {_count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}
}