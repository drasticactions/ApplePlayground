using System;
namespace ApplePlayground
{
    public class RootSplitViewController : UISplitViewController
    {
        private SidebarViewController sidebar;
        private BasicViewController testViewController;
        private NukeImageViewController nukeImageViewController;
        private UINavigationController navController;

        public RootSplitViewController()
            : base(UISplitViewControllerStyle.DoubleColumn)
        {
            var items = new List<SidebarListItem>();
            items.Add(new SidebarListItem() { Name = "Home" });
            items.Add(new SidebarListItem() { Name = "Nuke" });
            this.sidebar = new SidebarViewController(items);
            this.testViewController = new BasicViewController();
            this.navController = new UINavigationController(this.testViewController);
            this.nukeImageViewController = new NukeImageViewController();
            this.sidebar.OnItemSelected += this.Sidebar_OnItemSelected;
            this.PreferredDisplayMode = UISplitViewControllerDisplayMode.OneBesideSecondary;
            this.SetViewController(this.sidebar, UISplitViewControllerColumn.Primary);
            this.SetViewController(this.navController, UISplitViewControllerColumn.Secondary);

#if !TVOS
            this.PrimaryBackgroundStyle = UISplitViewControllerBackgroundStyle.Sidebar;
#endif
        }

        private void Sidebar_OnItemSelected(object? sender, SidebarSelectionEventArgs e)
        {
            switch (e.Item.Name)
            {
                case "Home":
                    this.navController.ViewControllers = new UIViewController[1] { this.testViewController };
                    this.navController.PopToRootViewController(false);
                    break;
                case "Nuke":
                    this.navController.ViewControllers = new UIViewController[1] { this.nukeImageViewController };
                    this.navController.PopToRootViewController(false);
                    break;
            }

#if IOS
            this.ShowColumn(UISplitViewControllerColumn.Secondary);
#endif
        }
    }
}

