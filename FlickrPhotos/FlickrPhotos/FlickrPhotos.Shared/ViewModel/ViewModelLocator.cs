using System;
using System.Collections.Generic;
using System.Text;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace FlickrPhotos.ViewModel
{
    class ViewModelLocator
    {
        //private FlickrAuthentication _flickrAuthenticationViewModel;
        private INavigationService _navigationService;
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            var _navigationService = this.CreateNavigationService();
            SimpleIoc.Default.Register<INavigationService>(() => _navigationService);
            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<FlickrAuthentication>();
            SimpleIoc.Default.Register<FlickrPhotosViewModel>();
        }
        public FlickrAuthentication FlickrAuthenticationViewModel
        {
            get { return ServiceLocator.Current.GetInstance<FlickrAuthentication>(); }
        }

        public FlickrPhotosViewModel FlickrPhotosViewModel
        {
            get { return ServiceLocator.Current.GetInstance<FlickrPhotosViewModel>(); }
        }

        private INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
            navigationService.Configure("FlickrAuthentication", typeof(FlickrAuthenticationView));
            navigationService.Configure("FlickrPhotos", typeof(Views.FlickrPhotos));
            // navigationService.Configure("key1", typeof(OtherPage1));
            // navigationService.Configure("key2", typeof(OtherPage2));

            return navigationService;
        }
    }
}
