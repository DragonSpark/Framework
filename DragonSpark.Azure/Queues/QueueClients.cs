using Azure.Storage.Queues;

namespace DragonSpark.Azure.Queues
{
	public sealed class QueueClients : IQueueClients
	{
		readonly string _connection;

		public QueueClients(AzureStorageConfiguration configuration) : this(configuration.Connection) {}

		public QueueClients(string connection) => _connection = connection;

		public QueueClient Get(string parameter) => new QueueClient(_connection, parameter);
	}
}