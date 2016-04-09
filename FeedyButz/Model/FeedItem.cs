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

namespace FeedyButz.Model
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
        public FeedItem(string title, string url)
        {
            Title = title;
            Url = url;
        }

        public FeedItem(string title, string url, string description)
        {
            Title = title;
            Url = url;
            Description = description;
        }

        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; } = "";
    }

    public class FeedItemManager
    {
        public static async Task<int> RequestFeed(string feedUrl, ObservableCollection<FeedItem> feedItems)
        {
            int addedItems = 0;
            Windows.Data.Xml.Dom.XmlDocument xmlDoc = new Windows.Data.Xml.Dom.XmlDocument();
            HttpWebRequest request = HttpWebRequest.CreateHttp(feedUrl);

            WebResponse response = await request.GetResponseAsync();
            Stream s = response.GetResponseStream();
            StreamReader r = new StreamReader(s);
            xmlDoc.LoadXml(r.ReadToEnd());

            Uri uri = new Uri(feedUrl);
            feedItems.Add(new FeedItem(uri.Host, feedUrl));

            addedItems = ParseRSS(xmlDoc, feedItems);
            if (addedItems == 0)
                addedItems = ParseAtom(xmlDoc, feedItems);

            return addedItems;
        }

        private static int ParseAtom(Windows.Data.Xml.Dom.XmlDocument xmlDoc, ObservableCollection<FeedItem> feedItems)
        {
            int addedItems = 0;
            string atomNS = "xmlns:atom='http://www.w3.org/2005/Atom'";
            Windows.Data.Xml.Dom.XmlNodeList atomNodes = xmlDoc.SelectNodesNS("atom:feed/atom:entry", atomNS);

            foreach (Windows.Data.Xml.Dom.IXmlNode atomNode in atomNodes)
            {
                Windows.Data.Xml.Dom.IXmlNode atomSubNode = atomNode.SelectSingleNodeNS("atom:title", atomNS);
                string title = atomSubNode != null ? atomSubNode.InnerText : "";

                atomSubNode = atomNode.SelectSingleNodeNS("atom:link", atomNS);
                string link = atomSubNode.Attributes.Where(a => a.NodeName == "href").First().InnerText;

                atomSubNode = atomNode.SelectSingleNodeNS("atom:summary", atomNS);
                string summary = atomSubNode != null ? atomSubNode.InnerText : "";

                feedItems.Add(new FeedItem(title, link, summary));
                addedItems++;
            }

            return addedItems;
        }

        private static int ParseRSS(Windows.Data.Xml.Dom.XmlDocument xmlDoc, ObservableCollection<FeedItem> feedItems)
        {
            int addedItems = 0;
            Windows.Data.Xml.Dom.XmlNodeList rssNodes = xmlDoc.SelectNodes("rss/channel/item");

            foreach (Windows.Data.Xml.Dom.IXmlNode rssNode in rssNodes)
            {
                Windows.Data.Xml.Dom.IXmlNode rssSubNode = rssNode.SelectSingleNode("title");
                string title = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("link");
                string link = rssSubNode != null ? rssSubNode.InnerText : "";

                rssSubNode = rssNode.SelectSingleNode("description");
                string description = rssSubNode != null ? rssSubNode.InnerText : "";

                feedItems.Add(new FeedItem(title, link, description));
                addedItems++;
            }

            return addedItems;
        }
    }
}
