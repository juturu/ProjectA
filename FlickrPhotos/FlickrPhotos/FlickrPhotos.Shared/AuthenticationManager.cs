using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using FlickrNet;
using Windows.Storage;

namespace FlickrPhotos
{
    public class AuthenticationManager
    {
        public const string k_strApiKey = "74aee8f9e88789449c75a69df45cf9a7";
        public const string k_strSharedSecret = "eb59b953e12da663";
        private static ApplicationDataContainer s_localSettings = ApplicationData.Current.LocalSettings;
        public static Flickr Instance
        {
            get
            {
                return new Flickr(k_strApiKey, k_strSharedSecret);
            }
        }

        public static Flickr AuthInstance
        {
            get
            {
                var f = new Flickr(k_strApiKey, k_strSharedSecret);
                f.OAuthAccessToken = OAuthToken;
                f.OAuthAccessTokenSecret = OAuthTokenSecret;
                return f;
            }
        }


        public static string OAuthToken
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

        public static string OAuthTokenSecret
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


    }
}
