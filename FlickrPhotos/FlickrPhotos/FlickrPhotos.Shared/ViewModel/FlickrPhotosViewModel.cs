using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FlickrPhotos.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace FlickrPhotos.ViewModel
{
    class FlickrPhotosViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        private ObservableCollection<Photo> _photosCollection = new ObservableCollection<Photo>();

        private FlickrPhotoManager _flickrPhotoManager = new FlickrPhotoManager();
        //public FlickrPhotosViewModel()
        //{
            
        //}

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
