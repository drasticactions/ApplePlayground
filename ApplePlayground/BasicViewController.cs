using System;
using UIKit;

namespace ApplePlayground
{
	public class BasicViewController : UIViewController
	{
		public BasicViewController()
        {
			this.View!.AddSubview(new UILabel(View!.Frame)
            {
#if !TVOS
                BackgroundColor = UIColor.SystemBackground,
#endif
                TextAlignment = UITextAlignment.Center,
                Text = "Hello, Apple!",
                AutoresizingMask = UIViewAutoresizing.All,
            });
        }
	}
}

