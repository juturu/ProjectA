using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
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
            _photosCollection.Clear();
            //f.PhotosetsGetListAsync(async (r) =>
            //{
            //    PhotosetCollection photosetCollection = r.Result;

            //    foreach (Photoset p in photosetCollection)
            //    {
            //        f.PhotosetsGetPhotosAsync(p.PhotosetId, async (photos) =>
            //        {
            //            PhotosetPhotoCollection photoCollection = photos.Result;

            //            foreach (FlickrNet.Photo photo in photoCollection)
            //            {
            //                _photosCollection.Add(new Photo(new Uri(photo.LargeSquareThumbnailUrl),
            //                    new Uri(photo.LargeUrl), photo.Title, ImageSource.Flickr));
            //            }

            //            callback(_photosCollection);
            //        });
            //    }
            //});

            // Recent Photos on Flickr
            f.PhotosGetRecentAsync(0, 20, (r) =>
            {
                PhotoCollection photoCollection = r.Result;
                foreach (FlickrNet.Photo p in photoCollection)
                {
                    _photosCollection.Add(new Photo(new Uri(p.LargeSquareThumbnailUrl),
                                new Uri(p.LargeUrl), p.Title, ImageSource.Flickr));
                }
                callback(_photosCollection);
            });
        }

        // Move this to its own class
        internal async Task DownloadPhotosAsync(IList<object> _selectedPhotos)
        {
            foreach (object photo in _selectedPhotos)
            {
                var temp = photo as Photo;
                
                StorageFile destinationFile = await KnownFolders.PicturesLibrary.CreateFileAsync(
            temp.Name + Path.GetExtension(temp.FullImageUri.ToString()), CreationCollisionOption.GenerateUniqueName);

                BackgroundDownloader downloader = new BackgroundDownloader();
                DownloadOperation download = downloader.CreateDownload(temp.FullImageUri, destinationFile);

                await HandleDownloadAsync(download, true);
            }
        }

        private async Task HandleDownloadAsync(DownloadOperation download, bool start)
        {
            try
            {
                // Store the download so we can pause/resume.
                //activeDownloads.Add(download);

                //Progress<DownloadOperation> progressCallback = new Progress<DownloadOperation>(DownloadProgress);
                if (start)
                {
                    // Start the download and attach a progress handler.
                    await download.StartAsync().AsTask();//cts.Token, progressCallback);
                }
                else
                {
                    // The download was already running when the application started, re-attach the progress handler.
                    await download.AttachAsync().AsTask(); //cts.Token, progressCallback);
                }

                ResponseInformation response = download.GetResponseInformation();
                //Log(String.Format("Completed: {0}, Status Code: {1}", download.Guid, response.StatusCode));
            }
            catch (TaskCanceledException)
            {
                //Log("Download cancelled.");
            }
            catch (Exception ex)
            {
                //LogException("Error", ex);
            }
            finally
            {
                //activeDownloads.Remove(download);
            }
        }
    }
}
