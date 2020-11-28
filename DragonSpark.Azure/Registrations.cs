using Azure.Storage.Blobs;
using DragonSpark.Azure.Queues;
using DragonSpark.Azure.Storage;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Azure
{
	sealed class Registrations : ICommand<IServiceCollection>
	{
		public static Registrations Default { get; } = new Registrations();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Register<AzureStorageConfiguration>()
			         //
			         .Start<BlobServiceClient>()
			         .Use<ConfiguredStorageClient>()
			         .Singleton()
			         //
			         .Then.Start<IStorageContainers>()
			         .Forward<StorageContainers>()
			         .Decorate<ValidatedStorageContainers>()
			         .Singleton()
			         //
			         .Then.Start<IQueueClients>()
			         .Forward<QueueClients>()
			         .Decorate<ValidatedQueueClients>()
			         .Singleton()
				;
		}
	}
}