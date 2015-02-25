using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using FlickrNet;
using FlickrPhotos.Interfaces;
using FlickrPhotos.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace FlickrPhotos.ViewModel
{
    class FlickrAuthentication : ViewModelBase
    {
        private INavigationService _navigationService;
        private readonly IAuthenticationManager<Flickr> _flickerAuthenticationManager = new FlickrAuthenticationManager();
        private Uri _uri = null;

        public RelayCommand FlickrPhotosCommand { get; set; }

        public FlickrAuthentication(INavigationService navigationService)
        {
            _navigationService = navigationService;

            FlickrPhotosCommand = new RelayCommand(() =>
            {
                _navigationService.NavigateTo("FlickrPhotos", null);
            });
        }
        public bool Visible
        {
            get
            {
                //bool visible = !_flickerAuthenticationManager.IsAuthenticated;
                //if (!visible)
                //    FlickrPhotosCommand.Execute(null);
                return !_flickerAuthenticationManager.IsAuthenticated;
            }
            set
            {
                Visible = value;
                RaisePropertyChanged();
            }
        }

        public Uri AuthenticationUri
        {
            get
            {
                if (_uri == null)
                {
                    _flickerAuthenticationManager.GetAuthenticationUriAsync(r =>
                    {
                        _uri = r;
                        RaisePropertyChanged();
                    });
                }

                return _uri;
            }
            set
            {
                _uri = value;
                RaisePropertyChanged();
            }
        }

        
        public ICommand AuthenticateCommand
        {
            get
            {
                return new RelayCommand<string>(Authenticate);
            }
        }
        public void Authenticate(string strOAuthVerifier)
        {
            //var strOAuthVerifier = oAuthVerifier as string;
            // Get Flickr Authentication Url

            _flickerAuthenticationManager.UpdateAccessTokenAsync(strOAuthVerifier, r =>
            {
                if (r != null)
                {
                    throw new Exception("Authentication Failed!");
                }
                else
                {
                    FlickrPhotosCommand.Execute(null);
                }
            });
        }
        
    }
}
