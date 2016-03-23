using FeedyButz.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;

namespace FeedyButz
{
    class SettingsManager
    {
        private static List<Feed> _defaultFeeds = new List<Feed>()
            {
                new Feed("http://heise.de.feedsportal.com/c/35207/f/653902/index.rss"),
                new Feed("http://golem.de.dynamic.feedsportal.com/pf/578068/http://rss.golem.de/rss.php?feed=RSS1.0"),
                new Feed("http://www.faz.net/rss/aktuell/"),
            };

        public static void RestoreSettings()
        {
            Application.Current.Resources["Feeds"] = SettingsManager.ReadFeedSettings();
        }

        public static IList<Feed> GetCurrentFeeds()
        {
            return (IList<Feed>)Application.Current.Resources["Feeds"];
        }

        public static void SetCurrentFeeds(IList<Feed> feeds)
        {
            Application.Current.Resources["Feeds"] = feeds;
        }

        public static void StoreFeedSettings(IList<Feed> feeds)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            localSettings.Values.Clear();
            for (int i = 0; i < feeds.Count(); i++)
                localSettings.Values["feed" + i] = feeds[i].Url;
        }

        public static IList<Feed> ReadFeedSettings()
        {
            List<Feed> feeds = new List<Feed>();
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            int i = 0;
            while (true)
            {
                var u = localSettings.Values["feed" + i];
                if (u == null)
                    break;
                else
                    feeds.Add(new Feed((string)u));
                i++;
            }

            if (feeds.Count() == 0)
                feeds = _defaultFeeds;

            return feeds;
        }
    }
}
