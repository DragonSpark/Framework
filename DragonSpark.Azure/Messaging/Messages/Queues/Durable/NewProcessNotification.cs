using DragonSpark.Application.AspNet.Workers;
using DragonSpark.Compose;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Runtime;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class NewProcessNotification : IStopAware<DurableMessageProperties, ProcessNotification?>
{
	readonly LocateExternalProcessReference _process;
	readonly ITime                          _time;

	public NewProcessNotification(LocateExternalProcessReference process) : this(process, Time.Default) {}

	public NewProcessNotification(LocateExternalProcessReference process, ITime time)
	{
		_process = process;
		_time    = time;
	}

	public async ValueTask<ProcessNotification?> Get(Stop<DurableMessageProperties> parameter)
	{
		var ((identifier, _, destination, visibility, life), stop) = parameter;
		var now     = _time.Get();
		var process = await _process.Off(new(identifier.Value(), stop));
		return process is not null
			       ? new()
			       {
				       Subject     = process,
				       Destination = destination,
				       Created     = now,
				       AvailableAt = visibility.HasValue ? now + visibility.Value : null,
				       Lifetime    = life
			       }
			       : null;
	}
}