using DragonSpark.Compose;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class Process : IProcess
{
	readonly CreateProcessNotification _create;

	public Process(CreateProcessNotification create) => _create = create;

	public async ValueTask Get(Stop<DurableMessageProperties> parameter)
	{
		await _create.Off(parameter);
	}
}