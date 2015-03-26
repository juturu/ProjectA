using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Windows.UI.Xaml.Controls;
using FlickrPhotos.Common;
using FlickrPhotos.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace FlickrPhotos.ViewModel
{
    class FlickrAlbumsViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private ObservableCollection<Album> _albumsCollection;

        private RelayCommand<ItemClickEventArgs> _albumClickedCommand;
        private FlickrPhotoManager _flickrPhotoManager = new FlickrPhotoManager();

        public FlickrAlbumsViewModel(INavigationService navigationService)
        {
            _flickrPhotoManager.GetAlbums(r => { AlbumsCollection = r; });
            _navigationService = navigationService;
        }

        public ObservableCollection<Album> AlbumsCollection
        {
            get { return _albumsCollection; }
            set
            {
                _albumsCollection = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand<ItemClickEventArgs> AlbumClickedCommand
        {
            get
            {
                if (_albumClickedCommand == null)
                {
                    _albumClickedCommand = new RelayCommand<ItemClickEventArgs>((e) =>
                    {
                        Album album = e.ClickedItem as Album;
                        _navigationService.NavigateTo("FlickrPhotos", album);
                    });
                }

                return _albumClickedCommand;
            }
        }
    }
}
