using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using FlickrNet;
using FlickrPhotos.ViewModel;

namespace FlickrPhotos
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlickrAuthenticationView : Page
    {
        // A dummy callback url - as long as this is a valid URL it doesn't matter what it is
        private const string k_callbackUrl = "http://localhost/dummy";
        // The request token, held while the authentication is completed.
        private OAuthRequestToken requestToken = null;

        public FlickrAuthenticationView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //Flickr f = AuthenticationManager.AuthInstance;

            //// obtain the request token from Flickr
            
            //f.OAuthGetRequestTokenAsync(k_callbackUrl, async (r) =>
            //{
            //    // Check if an error was returned
            //    if (r.Error != null)
            //    {
            //        //Dispatcher.BeginInvoke(() => { MessageBox.Show("An error occurred getting the request token: " + r.Error.Message); });
            //        return;
            //    }

            //    // Get the request token
            //    requestToken = r.Result;

            //    // get Authorization url
            //    string url = f.OAuthCalculateAuthorizationUrl(requestToken.Token, AuthLevel.Write);
            //    // Replace www.flickr.com with m.flickr.com for mobile version
            //    // url = url.Replace("https://www.flickr.com", "http://www.flickr.com");

            //    // Navigate to url
            //    CoreDispatcher dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            //    await
            //        dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            //            () =>
            //            {
            //                Debug.Assert(FlickrAuthenticationWebView != null, "FlickrAuthenticationWebView != null");
            //                FlickrAuthenticationWebView.Navigate(new Uri(url));
            //            });

            //});

            //f.PhotosetsGetListAsync(async (r) =>
            //{
            //    PhotosetCollection photosetCollection = r.Result;

            //    foreach (Photoset p in photosetCollection)
            //    {
            //        f.PhotosetsGetPhotosAsync(p.PhotosetId, async (photos) =>
            //        {
            //            PhotosetPhotoCollection photoCollection = photos.Result;

            //            //photoCollection[0].Large1600Url
            //            CoreDispatcher dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            //            await
            //                dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            //                    () =>
            //                    {
            //                        WebViewPopup.IsOpen = false;
            //                        PhotoItems.ItemsSource = photoCollection;
            //                    });
                        
            //        });
            //    }
                
            //});
            
        }

        private void FlickrAuthentication_OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Url: " + e.Uri.AbsoluteUri);

            // if we are not navigating to the callback url then authentication is not complete.
            if (!e.Uri.AbsoluteUri.StartsWith(k_callbackUrl)) return;

            // Get "oauth_verifier" part of the query string.
            var oauthVerifier = e.Uri.Query.Split('&')
                .Where(s => s.Split('=')[0] == "oauth_verifier")
                .Select(s => s.Split('=')[1])
                .FirstOrDefault();

            if (String.IsNullOrEmpty(oauthVerifier))
            {
                //MessageBox.Show("Unable to find Verifier code in uri: " + e.Uri.AbsoluteUri);
                System.Diagnostics.Debug.WriteLine("Unable to find Verifier code in uri:" + e.Uri.AbsoluteUri);
                return;
            }

            // Found verifier, so cancel navigation
            e.Cancel = true;
            FlickrAuthenticationWebView.Visibility = Visibility.Collapsed;

            // Obtain the access token from Flickr
            Flickr f = AuthenticationManager.AuthInstance;

            f.OAuthGetAccessTokenAsync(requestToken, oauthVerifier, r =>
            {
                // Check if an error was returned
                if (r.Error != null)
                {
                    //Dispatcher.BeginInvoke(() => MessageBox.Show("An error occurred getting the access token: " + r.Error.Message));
                    System.Diagnostics.Debug.WriteLine("An error occurred getting the access token:" + r.Error.Message);
                    return;
                }

                OAuthAccessToken accessToken = r.Result;

                // Save the oauth token for later use
                AuthenticationManager.OAuthToken = accessToken.Token;
                AuthenticationManager.OAuthTokenSecret = accessToken.TokenSecret;

                //Dispatcher.BeginInvoke(() => MessageBox.Show("Authentication completed for user " + accessToken.FullName + ", with token " + accessToken.Token));

            });

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }

        private void FlickrAuthenticationView_OnLoaded(object sender, RoutedEventArgs e)
        {
            // ToDO: Remove this and move this logic out to app startup.
            FlickrAuthentication dataContext = (sender as FlickrAuthenticationView).DataContext as FlickrAuthentication;
            if (!dataContext.Visible)
            {
                dataContext.FlickrPhotosCommand.Execute(null);
            }
        }
    }
}
