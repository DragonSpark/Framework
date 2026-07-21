using DragonSpark.Compose;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop.Conditions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class ProcessMessage : IDepending<DurableMessageProperties>
{
	readonly IProcess                _process;
	readonly ILogger<ProcessMessage> _logger;

	public ProcessMessage(IProcess process, ILogger<ProcessMessage> logger)
	{
		_process = process;
		_logger  = logger;
	}

	public async ValueTask<bool> Get(Stop<DurableMessageProperties> parameter)
	{
		var (subject, stop) = parameter;
		try
		{
			await _process.Off(parameter);
		}
		catch (OperationCanceledException) when (stop.IsCancellationRequested)
		{
			return false;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex,
			                 "Failed to process distributed message for Notification ID {Id} targeting {Queue}",
			                 subject.Identifier, subject.Destination);
		}

		return true;
	}
}