using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FeedReader.Model
{
    public class FeedItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }

    public class FeedItemManager
    {
        public static async void GetFeedUrls(string feedUrl, ObservableCollection<FeedItem> feedItems)
        {
            Windows.Data.Xml.Dom.XmlDocument xmlDoc = new Windows.Data.Xml.Dom.XmlDocument();
            HttpWebRequest request = HttpWebRequest.CreateHttp(feedUrl);

            WebResponse response = await request.GetResponseAsync();
            Stream s = response.GetResponseStream();
            StreamReader r = new StreamReader(s);
            xmlDoc.LoadXml(r.ReadToEnd());

            Windows.Data.Xml.Dom.XmlNodeList rssNodes = xmlDoc.SelectNodes("rss/channel/item");

            // Iterate through the items in the RSS file
            foreach (Windows.Data.Xml.Dom.IXmlNode rssNode in rssNodes)
            {
                Windows.Data.Xml.Dom.IXmlNode rssSubNode = rssNode.SelectSingleNode("title");
                string title = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("link");
                string link = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("description");
                string description = rssSubNode != null ? rssSubNode.InnerText : "";

                feedItems.Add(new FeedItem() { Title = title, Url = link });
            }
        }
    }
}
