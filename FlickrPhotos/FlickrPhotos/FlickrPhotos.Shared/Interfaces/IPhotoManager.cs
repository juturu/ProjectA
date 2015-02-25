using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FlickrPhotos.Model;

namespace FlickrPhotos.Interfaces
{
    interface IPhotoManager
    {
        void GetPhotos(Action<ObservableCollection<Photo>> callback);
    }
}
