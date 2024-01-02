using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events.Receive;

public sealed class ProcessError : IAllocated<ProcessErrorEventArgs>
{
	readonly Error _error;

	public ProcessError(Error error) => _error = error;

	public Task Get(ProcessErrorEventArgs parameter)
	{
		_error.Execute(parameter.Exception, parameter.PartitionId, parameter.Operation);
		return Task.CompletedTask;
	}

	public sealed class Error : LogErrorException<string, string>
	{
		public Error(ILogger<Error> logger)
			: base(logger, "An exception occurred on {Partition} while performing {Operation}") {}
	}
}