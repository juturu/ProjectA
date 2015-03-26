using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FlickrPhotos.Model
{
    class Album
    {
        public string Name { get; set; }

        public ObservableCollection<AlbumPhoto> AlbumPhotos { get; set; }
    }
}
