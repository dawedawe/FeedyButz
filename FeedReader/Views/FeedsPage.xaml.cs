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
using FeedReader.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FeedReader.Views
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
            List<string> feeds = new List<string>()
            {
                "http://heise.de.feedsportal.com/c/35207/f/653902/index.rss",
                "http://golem.de.dynamic.feedsportal.com/pf/578068/http://rss.golem.de/rss.php?feed=RSS1.0",
                "http://www.faz.net/rss/aktuell/",
            };
            Application.Current.Resources["Feeds"] = feeds;
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
            List<string> feeds = (List<string>)Application.Current.Resources["Feeds"];
            foreach (string feed in feeds)
                await FeedItemManager.GetFeedUrls(feed, _feedItems);
            ReloadProgressRing.IsActive = false;
            FeedsGrid.RowDefinitions[0].Height = new GridLength(0);
        }
    }
}
