using System;
using System.Collections.Generic;
using System.Text;

namespace FlickrPhotos.Model
{
    class Photo
    {
        public Uri ThumbNailUri { get; set; }
        public Uri FullImageUri { get; set; }

        public string Name { get; set; }
        public ImageSource ImageSource { get; set; }

        public Photo(Uri thumbnailUri, Uri fullImageUri, string name, ImageSource imageSource)
        {
            ThumbNailUri = thumbnailUri;
            FullImageUri = fullImageUri;
            Name = name;
            ImageSource = imageSource;
        }
    }
}
