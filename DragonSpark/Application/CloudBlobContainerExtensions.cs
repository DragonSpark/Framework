using System.Net;
using Microsoft.WindowsAzure.StorageClient;

namespace DragonSpark.Application
{
    public static class CloudBlobContainerExtensions
    {
        public static bool Exists( this CloudBlob target )
        {
            try
            {
                target.FetchAttributes();
                return true;
            }
            catch ( StorageClientException ex )
            {
                var result = ex.StatusCode != HttpStatusCode.NotFound;
                return result;
            }
        }
    }
}