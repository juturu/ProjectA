using System;
using System.Collections.Generic;
using System.Text;
using FlickPhotos.Model;

namespace FlickrPhotos.Model
{
    class Comment
    {
        Person commentedBy;
        string Title;
        IEnumerable<Like> likes;
        DateTime creationTime;
    };
}
