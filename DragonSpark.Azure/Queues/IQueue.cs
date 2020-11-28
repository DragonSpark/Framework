using Azure.Storage.Queues;
using DragonSpark.Model.Results;

namespace DragonSpark.Azure.Queues
{
	public interface IQueue : IResult<QueueClient> {}
}