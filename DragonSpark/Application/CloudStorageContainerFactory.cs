using System;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace DragonSpark.Application
{
    public class CloudStorageContainerFactory : FactoryBase
    {
        public CloudStorageContainerFactory()
        {
            DefaultPermissions = BlobContainerPublicAccessType.Container;
        }

        public string ConnectionStringName { get; set; }

        public string ContainerName { get; set; }

        public BlobContainerPublicAccessType DefaultPermissions { get; set; }

        protected override object Create( IUnityContainer container, Type type, string buildName )
        {
            var client = CloudStorageAccount.FromConfigurationSetting( ConnectionStringName ).CreateCloudBlobClient();

            var result = client.GetContainerReference( ContainerName.ToLower() );
            if ( result.CreateIfNotExist() )
            {
                var permissions = new BlobContainerPermissions { PublicAccess = DefaultPermissions };
                result.SetPermissions( permissions );
            }

            return result;
        }
    }
}