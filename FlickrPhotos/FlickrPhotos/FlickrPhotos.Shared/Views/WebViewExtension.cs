using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using FlickrPhotos.ViewModel;

namespace FlickrPhotos.Views
{
    public static class WebViewExtensions
    {
        private const string k_callbackUrl = "http://localhost/dummy";
        public static string GetUriSource(WebView view)
        {
            return (string)view.GetValue(UriSourceProperty);
        }

        public static void SetUriSource(WebView view, string value)
        {
            view.SetValue(UriSourceProperty, value);
        }

        public static readonly DependencyProperty UriSourceProperty =
            DependencyProperty.RegisterAttached(
            "UriSource", typeof(string), typeof(WebViewExtensions),
            new PropertyMetadata(null, OnUriSourcePropertyChanged));

        private static void OnUriSourcePropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var webView = sender as WebView;
            if (webView == null)
                throw new NotSupportedException();

            if (e.NewValue != null)
            {
                var uri = new Uri(e.NewValue.ToString());
                webView.Navigate(uri);
                webView.NavigationStarting += (view, args) =>
                {
                    if (!args.Uri.AbsoluteUri.StartsWith(k_callbackUrl))
                    {
                        return;
                    }
                    else
                    {
                        var oauthVerifier = args.Uri.Query.Split('&')
                                                .Where(s => s.Split('=')[0] == "oauth_verifier")
                                                .Select(s => s.Split('=')[1])
                                                .FirstOrDefault();

                        if (String.IsNullOrEmpty(oauthVerifier))
                        {
                            //MessageBox.Show("Unable to find Verifier code in uri: " + e.Uri.AbsoluteUri);
                            System.Diagnostics.Debug.WriteLine("Unable to find Verifier code in uri:" + args.Uri.AbsoluteUri);
                            return;
                        }

                        var flickrAuthentication = webView.DataContext as FlickrAuthentication;
                        flickrAuthentication.Authenticate(oauthVerifier);

                        webView.Visibility = Visibility.Collapsed;
                    }
                };
            }
        }
    }
}
