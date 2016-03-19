using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace FeedReader.Model
{
    public class Feed
    {
        public Feed(string url)
        {
            Url = url;
        }

        public string Url { get; set; }
    }

    public class FeedItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }

    public class FeedItemManager
    {
        private static List<Feed> _defaultFeeds = new List<Feed>()
            {
                new Feed("http://heise.de.feedsportal.com/c/35207/f/653902/index.rss"),
                new Feed("http://golem.de.dynamic.feedsportal.com/pf/578068/http://rss.golem.de/rss.php?feed=RSS1.0"),
                new Feed("http://www.faz.net/rss/aktuell/"),
            };

        public static async Task<int> RequestFeed(string feedUrl, ObservableCollection<FeedItem> feedItems)
        {
            int addedItems = 0;
            Windows.Data.Xml.Dom.XmlDocument xmlDoc = new Windows.Data.Xml.Dom.XmlDocument();
            HttpWebRequest request = HttpWebRequest.CreateHttp(feedUrl);

            WebResponse response = await request.GetResponseAsync();
            Stream s = response.GetResponseStream();
            StreamReader r = new StreamReader(s);
            xmlDoc.LoadXml(r.ReadToEnd());

            Windows.Data.Xml.Dom.XmlNodeList rssNodes = xmlDoc.SelectNodes("rss/channel/item");
            feedItems.Add(new FeedItem() { Title = feedUrl, Url = feedUrl });

            // Iterate through the items in the RSS file
            foreach (Windows.Data.Xml.Dom.IXmlNode rssNode in rssNodes)
            {
                Windows.Data.Xml.Dom.IXmlNode rssSubNode = rssNode.SelectSingleNode("title");
                string title = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("link");
                string link = rssSubNode != null ? rssSubNode.InnerText : "";

                feedItems.Add(new FeedItem() { Title = title, Url = link });
                addedItems++;
            }

            return addedItems;
        }

        public static void StoreFeedSettings(IList<Feed> feeds)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

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
