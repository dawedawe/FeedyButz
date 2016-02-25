using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Data.Xml;
using Windows.Data.Xml.Dom;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FeedReader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            FeedItems = new ObservableCollection<FeedItem>();
        }

        private ObservableCollection<FeedItem> FeedItems;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RefreshFeeds();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshFeeds();
        }

        private void RefreshFeeds()
        {
            List<string> feeds = new List<string> {
                "http://heise.de.feedsportal.com/c/35207/f/653902/index.rss",
                "http://golem.de.dynamic.feedsportal.com/pf/578068/http://rss.golem.de/rss.php?feed=RSS1.0",
                "http://www.faz.net/rss/aktuell/",
            };

            FeedItems.Clear();
            foreach (string feed in feeds)
            {
                Model.FeedItemManager.GetFeedUrls(feed, FeedItems);
            }
        }
    }
}
