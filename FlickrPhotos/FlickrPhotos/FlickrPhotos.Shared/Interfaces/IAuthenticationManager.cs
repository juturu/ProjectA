using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlickrPhotos.Interfaces
{
    interface IAuthenticationManager<T>
    {
        bool IsAuthenticated { get; }
        T AuthInstance { get; }

        void GetAuthenticationUriAsync(Action<Uri> callback);

        void UpdateAccessTokenAsync(string strOAuthVerifier, Action<Exception> callback);
    }
}
