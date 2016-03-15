﻿using System;
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
using FeedReader.Views;

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
        }

        private void ReloadButton_Click(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(typeof(FeedsPage));
        }

        private void AddFeedButton_Click(object sender, RoutedEventArgs e)
        {
            if (FeedTextBox.Text.Length > 0)
            {
                List<string> feeds = (List<string>)Application.Current.Resources["Feeds"];
                feeds.Add(FeedTextBox.Text);
                FeedTextBox.Text = string.Empty;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MyFrame.Navigate(typeof(FeedsPage));
        }
    }
}
