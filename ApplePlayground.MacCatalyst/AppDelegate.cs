namespace ApplePlayground.MacCatalyst;

[Register ("AppDelegate")]
public class AppDelegate : UIApplicationDelegate {
	public override UIWindow? Window {
		get;
		set;
	}

	public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
	{
        Drastic.Rainbows.SwizzleCommands.Start();

        // create a new window instance based on the screen size
        Window = new UIWindow (UIScreen.MainScreen.Bounds);

        // create a UIViewController with a single UILabel
        Window.RootViewController = new RootSplitViewController();

        // make the window visible
        Window.MakeKeyAndVisible ();

		return true;
	}
}
