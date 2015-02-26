using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using FlickrPhotos.Model;
using GalaSoft.MvvmLight.Messaging;

namespace FlickrPhotos.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FlickrPhotos : Page
    {
        public FlickrPhotos()
        {
            this.InitializeComponent();
            Messenger.Default.Register<StoryboardMessage>(this, x => StartStoryboard(x.StoryboardName, x.LoopForever));
        }

        private void StartStoryboard(string storyboardName, bool loopForever)
        {
            var storyboard = FindName(storyboardName) as Storyboard;
            if (storyboard != null)
            {
                if (loopForever)
                    storyboard.RepeatBehavior = RepeatBehavior.Forever;
                else
                    storyboard.RepeatBehavior = new RepeatBehavior(1);
                storyboard.Begin();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //var parameter = e.Parameter as string;  // "My data"
            base.OnNavigatedTo(e);
        }
    }
}
