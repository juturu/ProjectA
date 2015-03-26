using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FlickPhotos.Model;

namespace FlickrPhotos.Model
{
    class PhotoFeed
    {
        ObservableCollection<Photo> photos;
        ObservableCollection<Comment> comments;
        ObservableCollection<Like> likes;
        DateTime creationTime;
    };
}
