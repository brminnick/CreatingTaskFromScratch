namespace TooManyCooks.Mobile;

public class App(AppShell shell) : Application
{
	readonly AppShell _shell = shell;

	protected override Window CreateWindow(IActivationState? activationState)
	{
		base.CreateWindow(activationState);

		return new Window(_shell);
	}
}