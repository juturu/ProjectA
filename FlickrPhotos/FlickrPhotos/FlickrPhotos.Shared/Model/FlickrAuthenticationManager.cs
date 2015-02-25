using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using FlickrNet;
using FlickrPhotos.Interfaces;

namespace FlickrPhotos.Model
{
    class FlickrAuthenticationManager : IAuthenticationManager<Flickr>
    {
        private const string k_callbackUrl = "http://localhost/dummy";
        // The request token, held while the authentication is completed.
        private OAuthRequestToken _requestToken = null;
        private const string k_strApiKey = "74aee8f9e88789449c75a69df45cf9a7";
        private const string k_strSharedSecret = "eb59b953e12da663";
        private ApplicationDataContainer s_localSettings = ApplicationData.Current.LocalSettings;

        private string OAuthToken
        {
            get
            {
                s_localSettings = ApplicationData.Current.LocalSettings;
                if (s_localSettings.Values.ContainsKey("OAuthToken"))
                    return s_localSettings.Values["OAuthToken"] as string;
                else
                    return null;
            }
            set
            {
                s_localSettings.Values["OAuthToken"] = value;
            }
        }

        private string OAuthTokenSecret
        {
            get
            {
                s_localSettings = ApplicationData.Current.LocalSettings;
                if (s_localSettings.Values.ContainsKey("OAuthTokenSecret"))
                    return s_localSettings.Values["OAuthTokenSecret"] as string;
                else
                    return null;
            }
            set
            {
                s_localSettings.Values["OAuthTokenSecret"] = value;
            }
        }
        public bool IsAuthenticated
        {
            get
            {
                return (!String.IsNullOrEmpty(OAuthTokenSecret) && 
                        !String.IsNullOrEmpty(OAuthToken));
            }
        }

        public Flickr AuthInstance
        {
            get
            {
                var f = new Flickr(k_strApiKey, k_strSharedSecret);
                f.OAuthAccessToken = OAuthToken;
                f.OAuthAccessTokenSecret = OAuthTokenSecret;
                return f;
            }
        }


        public void GetAuthenticationUriAsync(Action<Uri> callback)
        {
            Flickr f = this.AuthInstance;
            
            
            // obtain the request token from Flickr
            f.OAuthGetRequestTokenAsync(k_callbackUrl, r =>
            {
                string url = null;
                // Check if an error was returned
                if (r.Error != null)
                {
                    //Dispatcher.BeginInvoke(() => { MessageBox.Show("An error occurred getting the request token: " + r.Error.Message); });
                    return;
                }

                // Get the request token
                _requestToken = r.Result;

                // get Authorization url
                url = f.OAuthCalculateAuthorizationUrl(_requestToken.Token, AuthLevel.Write);
                // Replace www.flickr.com with m.flickr.com for mobile version
                // url = url.Replace("https://www.flickr.com", "http://www.flickr.com");
                url = url.Replace("https://www.flickr.com", "https://m.flickr.com");

                callback(new Uri(url));
            });
            
        }

        public void UpdateAccessTokenAsync(string strOAuthVerifier, Action<Exception> callback)
        {
            Flickr f = this.AuthInstance;
            f.OAuthGetAccessTokenAsync(_requestToken, strOAuthVerifier, r =>
            {
                // Check if an error was returned
                if (r.Error != null)
                {
                    System.Diagnostics.Debug.WriteLine("An error occurred getting the access token:" + r.Error.Message);
                }
                else
                {
                    OAuthAccessToken accessToken = r.Result;

                    // Save the oauth token for later use
                    this.OAuthToken = accessToken.Token;
                    this.OAuthTokenSecret = accessToken.TokenSecret;
                }

                callback(r.Error);
                //Dispatcher.BeginInvoke(() => MessageBox.Show("Authentication completed for user " + accessToken.FullName + ", with token " + accessToken.Token));
            });

        }
    }
}
