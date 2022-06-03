using Azure.Storage.Queues;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Queues;

sealed class Message : IMessage
{
	readonly QueueClient _client;

	public Message(QueueClient client) => _client = client;

	public async ValueTask Get(MessageInput parameter)
	{
		var (message, life, visibility) = parameter;
		await _client.SendMessageAsync(message, visibility, life).ConfigureAwait(false);
	}
}