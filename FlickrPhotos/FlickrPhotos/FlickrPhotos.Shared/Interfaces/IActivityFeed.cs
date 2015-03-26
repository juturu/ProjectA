using System;
using System.Collections.Generic;
using System.Text;
using FlickrPhotos.Model;

namespace FlickrPhotos.Interfaces
{
    interface IActivityFeed
    {
        int RetrieveActivityFeedsAsync(CircleType circleType,
	                                   DateTime startDateTime /* TODO: Default Last 10 days */,
	                                   DateTime endDateTime /* Default CurrentTime */,
	                                   Action<PhotoFeedSets> callback,
	                                   Action<IProgress<double>> progress);

        void CancelTransaction(int transactionID);
    }
}
