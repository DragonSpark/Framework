using Azure.Storage.Queues;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Queues
{
	sealed class Send : ISend
	{
		readonly QueueClient _client;
		readonly TimeSpan    _life;
		readonly TimeSpan?   _visibility;

		public Send(QueueClient client, TimeSpan life, TimeSpan? visibility)
		{
			_client     = client;
			_life       = life;
			_visibility = visibility;
		}

		public async ValueTask Get(string parameter)
		{
			await _client.SendMessageAsync(parameter, _visibility, _life).ConfigureAwait(false);
		}
	}
}