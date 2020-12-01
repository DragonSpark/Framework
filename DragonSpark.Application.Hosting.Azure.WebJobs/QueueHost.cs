using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Hosting.Azure.WebJobs
{
	public class QueueHost : IAllocated<string>
	{
		readonly IQueueApplication _queue;

		public QueueHost(IQueueApplication queue) => _queue = queue;

		public virtual Task Get(string parameter) => _queue.Get(parameter);
	}
}