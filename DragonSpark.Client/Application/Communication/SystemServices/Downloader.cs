using System;
using System.IO;
using System.Net;
using System.Threading;

namespace DragonSpark.Application.Communication.SystemServices
{
    public class Downloader : IDownloader
    {
        public Stream Retrieve( Uri uri )
        {
            Exception error = null;
            Stream result = null;
            var client = new WebClient();
            var reset = new ManualResetEvent( false );
            client.OpenReadCompleted += ( s, a ) =>
                                        {
                                            error = a.Error;
                                            result = error != null ? null : a.Result;
                                            reset.Set();
                                        };
            client.OpenReadAsync( uri );
            reset.WaitOne();
            if ( error != null )
            {
                throw error;
            }
            return result;
        }
    }
}