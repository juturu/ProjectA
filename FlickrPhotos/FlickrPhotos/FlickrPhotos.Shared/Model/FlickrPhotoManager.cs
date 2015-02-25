using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Windows.UI.Core;
using FlickrNet;
using FlickrPhotos.Interfaces;
using GalaSoft.MvvmLight;

namespace FlickrPhotos.Model
{
    internal class FlickrPhotoManager : IPhotoManager
    {
        private ObservableCollection<Photo> _photosCollection = new ObservableCollection<Photo>();

        public ObservableCollection<Photo> PhotosCollection
        {
            get { return _photosCollection; }
        }
        public void GetPhotos(Action<ObservableCollection<Photo>> callback)
        {
            FlickrAuthenticationManager flickrAuthenticationManager = new FlickrAuthenticationManager();
            Flickr f = flickrAuthenticationManager.AuthInstance;
            f.PhotosetsGetListAsync(async (r) =>
            {
                PhotosetCollection photosetCollection = r.Result;

                foreach (Photoset p in photosetCollection)
                {
                    f.PhotosetsGetPhotosAsync(p.PhotosetId, async (photos) =>
                    {
                        PhotosetPhotoCollection photoCollection = photos.Result;

                        foreach (FlickrNet.Photo photo in photoCollection)
                        {
                            _photosCollection.Add(new Photo(new Uri(photo.LargeSquareThumbnailUrl),
                                new Uri(photo.LargeUrl), photo.Title, ImageSource.Flickr));
                        }

                        //photoCollection[0].Large1600Url
                        //CoreDispatcher dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
                        //await
                        //    dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        //        () =>
                        //        {
                        //            WebViewPopup.IsOpen = false;
                        //            PhotoItems.ItemsSource = photoCollection;
                        //        });

                        callback(_photosCollection);
                    });
                }
            });

        }
    }
}
