using System;
using Drastic.PureLayout;
using ObjCRuntime;

namespace ApplePlayground
{
    public class SidebarListItem : NSObject
    {
        public string Name { get; set; } = "Temp";

        public string? SystemIcon { get; set; }
    }

    public class SidebarSelectionEventArgs : EventArgs
    {
        private readonly SidebarListItem item;

        /// <summary>
        /// Initializes a new instance of the <see cref="SidebarSelectionEventArgs"/> class.
        /// </summary>
        /// <param name="ex">Exception.</param>
        public SidebarSelectionEventArgs(SidebarListItem item)
        {
            this.item = item;
        }

        /// <summary>
        /// Gets the Exception.
        /// </summary>
        public SidebarListItem Item => this.item;
    }

    public class SidebarViewController : UIViewController, IUICollectionViewDelegate
    {
        private UICollectionView collectionView;

        private UICollectionViewDiffableDataSource<NSString, SidebarListItem>? dataSource;

        private List<SidebarListItem> Items { get; }

        public SidebarViewController(List<SidebarListItem> items)
        {
            this.collectionView = new UICollectionView(this.View!.Bounds, this.CreateLayout());
            this.collectionView.Delegate = this;

            this.View.AddSubview(this.collectionView);

            // Anchor collectionView
            this.collectionView.TranslatesAutoresizingMaskIntoConstraints = false;

            //var edgeInsets = new UIEdgeInsets(top: 100, left: 100, bottom: 100, right: 100);
            this.collectionView.AutoPinEdgesToSuperviewEdges();
            //this.collectionView.AutoPinEdgesToSuperviewMargins();
            //this.collectionView.AutoPinEdgesToSuperviewEdges();
            //this.collectionView.AutoPinEdge(ALEdge.Left, )
            //this.collectionView.AutoSetDimension(ALDimension.Width, 200);
            //this.collectionView.AutoPinEdgesToSuperviewSafeAreaWithInsets(UIEdgeInsets.Zero);

            this.ConfigureDataSource();

            this.Items = items;
            var snapshot = this.GetNavigationSnapshot(this.Items);
            this.SetupNavigationItems(this.GetNavigationSnapshot(this.Items));
        }

        public event EventHandler<SidebarSelectionEventArgs>? OnItemSelected;

        /// <summary>
        /// Item Selected.
        /// </summary>
        /// <param name="collectionView">Collection View.</param>
        /// <param name="indexPath">Index Path.</param>
        [Export("collectionView:didSelectItemAtIndexPath:")]
        protected async void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = this.Items[indexPath.Row];
            this.OnItemSelected?.Invoke(this, new SidebarSelectionEventArgs(item));
        }

        private void SetupNavigationItems(NSDiffableDataSourceSectionSnapshot<SidebarListItem> snapshot)
        {
            if (this.dataSource is null)
            {
                return;
            }

            // Add base sidebar items
            var sectionIdentifier = new NSString("base");
            this.dataSource.ApplySnapshot(snapshot, sectionIdentifier, false);
        }

        private NSDiffableDataSourceSectionSnapshot<SidebarListItem> GetNavigationSnapshot(IEnumerable<SidebarListItem> items)
        {
            var snapshot = new NSDiffableDataSourceSectionSnapshot<SidebarListItem>();

            snapshot.AppendItems(items.ToArray());

            return snapshot;
        }

        private class CustomCell : UICollectionViewListCell
        {
            public CustomCell()
            {
            }

            public CustomCell(NSCoder coder) : base(coder)
            {
            }

            public CustomCell(CGRect frame) : base(frame)
            {
            }

            protected CustomCell(NSObjectFlag t) : base(t)
            {
            }

            protected internal CustomCell(NativeHandle handle) : base(handle)
            {
            }

        }

        private void ConfigureDataSource()
        {
            var rowRegistration = UICollectionViewCellRegistration.GetRegistration(typeof(CustomCell),
                new UICollectionViewCellRegistrationConfigurationHandler((cell, indexpath, item) =>
                {
                    var sidebarItem = item as SidebarListItem;
                    if (sidebarItem is null)
                    {
                        return;
                    }

#if TVOS
                    var cfg = UIListContentConfiguration.CellConfiguration;
#else
                    var cfg = UIListContentConfiguration.SidebarCellConfiguration;
#endif
                    cfg.Text = sidebarItem.Name;
                    if (sidebarItem.SystemIcon is not null)
                    {
                        cfg.Image = UIImage.GetSystemImage(sidebarItem.SystemIcon);
                    }

                    cell.ContentConfiguration = cfg;
                })
             );

            if (this.collectionView is null)
            {
                throw new NullReferenceException(nameof(this.collectionView));
            }

            this.dataSource = new UICollectionViewDiffableDataSource<NSString, SidebarListItem>(this.collectionView,
                new UICollectionViewDiffableDataSourceCellProvider((collectionView, indexPath, item) =>
                {
                    var sidebarItem = item as SidebarListItem;
                    if (sidebarItem is null || this.collectionView is null)
                    {
                        throw new Exception();
                    }

                    return this.collectionView.DequeueConfiguredReusableCell(rowRegistration, indexPath, item);
                })
            );
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            if (View is null)
            {
                throw new NullReferenceException(nameof(View));
            }
        }

        private UICollectionViewLayout CreateLayout()
        {
#if TVOS
            var config = new UICollectionLayoutListConfiguration(UICollectionLayoutListAppearance.Grouped)
            {
                HeaderMode = UICollectionLayoutListHeaderMode.None,
            };
#else
            var config = new UICollectionLayoutListConfiguration(UICollectionLayoutListAppearance.Sidebar)
            {
                HeaderMode = UICollectionLayoutListHeaderMode.None,
                ShowsSeparators = true
            };
#endif

            var test = UICollectionViewCompositionalLayout.GetLayout(config);
            return test;
        }
    }
}

