namespace ApplePlayground.tvOS;

[Register ("AppDelegate")]
public class AppDelegate : UIApplicationDelegate {
	public override UIWindow? Window {
		get;
		set;
	}

	public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
	{
        Drastic.Rainbows.SwizzleCommands.Start();

        Window = new UIWindow(UIScreen.MainScreen.Bounds);

        Window.RootViewController = new NukeImageViewController();

		Window.MakeKeyAndVisible();

        return true;
    }
}
