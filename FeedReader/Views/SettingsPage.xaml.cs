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
using System.Diagnostics;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace FeedReader.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();

            _feeds = new ObservableCollection<Feed>();
            IList<Feed> feeds = (IList<Feed>)Application.Current.Resources["Feeds"];
            foreach (var feed in feeds)
                _feeds.Add(feed);
        }

        private ObservableCollection<Feed> _feeds;

        private void AddFeedButton_Click(object sender, RoutedEventArgs e)
        {
            if (FeedTextBox.Text.Length > 0)
            {
                _feeds.Add(new Feed(FeedTextBox.Text));
                FeedTextBox.Text = string.Empty;
                Application.Current.Resources["Feeds"] = _feeds.ToList<Feed>();
            }
        }

        private void RemoveFeedButton_Click(object sender, RoutedEventArgs e)
        {
            if (feedsListBox.SelectedIndex >= 0)
                _feeds.RemoveAt(feedsListBox.SelectedIndex);

            Application.Current.Resources["Feeds"] = _feeds.ToList();
        }
    }
}
