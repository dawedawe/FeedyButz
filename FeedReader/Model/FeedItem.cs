using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
        public static async Task<int> GetFeedUrls(string feedUrl, ObservableCollection<FeedItem> feedItems)
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
    }
}
