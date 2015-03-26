using FlickPhotos.Model;
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

        public string Title {get; set;}
        public IEnumerable<Like> Likes {get; set;}
        public IEnumerable<Comment> Comments {get; set;}
        public DateTime CreationTime {get; set;}
    }
}
