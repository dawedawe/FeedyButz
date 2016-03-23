using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FeedyButz.Model;
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FeedyButz.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FeedsPage : Page
    {
        public FeedsPage()
        {
            this.InitializeComponent();
            _feedItems = new ObservableCollection<FeedItem>();
        }

        private ObservableCollection<FeedItem> _feedItems;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshFeeds();
        }

        private async void RefreshFeeds()
        {
            FeedsGrid.RowDefinitions[0].Height = GridLength.Auto;
            ReloadProgressRing.IsActive = true;
            _feedItems.Clear();

            IList<Feed> feeds = SettingsManager.GetCurrentFeeds();
            foreach (Feed feed in feeds)
            {
                try
                {
                    await FeedItemManager.RequestFeed(feed.Url, _feedItems);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("RefreshFeeds(): " + ex.Message);
                }
            }

            ReloadProgressRing.IsActive = false;
            FeedsGrid.RowDefinitions[0].Height = new GridLength(0);
        }
    }
}
