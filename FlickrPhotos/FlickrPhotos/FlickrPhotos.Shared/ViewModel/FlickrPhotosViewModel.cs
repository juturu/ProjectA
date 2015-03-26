using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using FlickrPhotos.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;

namespace FlickrPhotos.ViewModel
{
    class FlickrPhotosViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private ObservableCollection<Photo> _photosCollection = new ObservableCollection<Photo>();

        private FlickrPhotoManager _flickrPhotoManager = new FlickrPhotoManager();

        private RelayCommand<ItemClickEventArgs> _photoClickedCommand;
        private RelayCommand _goBackCommand;
        private RelayCommand _refreshCommand;
        private RelayCommand<SelectionChangedEventArgs> _photoSelectionChanged;
        private RelayCommand _downloadPhotosCommand;
        private RelayCommand _albumsViewCommand;
        private Photo _selectedPhoto;
        private IList<Object> _selectedPhotos;
        public Photo SelectedPhoto
        {
            get
            {
                return _selectedPhoto;
            }
        }

        public RelayCommand<ItemClickEventArgs> PhotoClickedCommand
        {
            get
            {
                if (_photoClickedCommand == null)
                {
                    _photoClickedCommand = new RelayCommand<ItemClickEventArgs>((photo) =>
                    {
                        Messenger.Default.Send<StoryboardMessage>(new StoryboardMessage
                        {
                            StoryboardName = "ImageEnterStoryboard",
                            LoopForever = false
                        });
                        // Navigate to Photo page with the photo context
                        _selectedPhoto = photo.ClickedItem as Photo;
                        _navigationService.NavigateTo("PhotosView", _selectedPhoto);
                    });
                }
                return _photoClickedCommand;
            }
        }

        public RelayCommand GoBackCommand
        {
            get
            {
                if (_goBackCommand == null)
                {
                    _goBackCommand = new RelayCommand(()=> {_navigationService.GoBack();});
                }

                return _goBackCommand;
            }
        }

        public RelayCommand RefreshCommand
        {
            get
            {
                if (_refreshCommand == null)
                {
                    _refreshCommand = new RelayCommand(() => { _flickrPhotoManager.GetPhotos(r => { PhotosCollection = r; }); });
                }

                return _refreshCommand;
            }
        }

        public RelayCommand<SelectionChangedEventArgs> PhotoSelectionChangedCommand
        {
            get
            {
                if (_photoSelectionChanged == null)
                {
                    _photoSelectionChanged = new RelayCommand<SelectionChangedEventArgs>((e) =>
                    {
                        // Bring up app bar (if required)
                        // Mark the selected items
                        if (e != null)
                        {
                            _selectedPhotos = e.AddedItems;
                        }
                        
                    });
                }
                return _photoSelectionChanged;
            }
        }

        public RelayCommand DownloadPhotosCommand
        {
            get
            {
                if (_downloadPhotosCommand == null)
                {
                    _downloadPhotosCommand = new RelayCommand(async () =>
                    {
                        // Begin download of selected photos
                        if (_selectedPhoto != null && _selectedPhotos.Count != 0)
                        {
                            await _flickrPhotoManager.DownloadPhotosAsync(_selectedPhotos);    
                        }

                        // Clear Selection
                    });
                }
                return _downloadPhotosCommand;
            }
        }

        public RelayCommand AlbumsViewCommand
        {
            get
            {
                if (_albumsViewCommand == null)
                {
                    _albumsViewCommand = new RelayCommand(() =>
                    {
                        _navigationService.NavigateTo("FlickrAlbums");
                    });
                }

                return _albumsViewCommand;
            }
        }
        public FlickrPhotosViewModel(INavigationService navigationService)
        {
            _flickrPhotoManager.GetPhotos(r => { PhotosCollection = r; });
            _navigationService = navigationService;
        }
        public ObservableCollection<Photo> PhotosCollection
        {
            get
            {
                return _photosCollection;
            }
            set 
            { 
                _photosCollection = value; 
                RaisePropertyChanged();
            }
        }
    }
}
