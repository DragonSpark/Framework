using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop.Conditions;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class ProcessMessage : IDepending<DurableMessageProperties>
{
	readonly ISendMessage            _send;
	readonly ILogger<ProcessMessage> _logger;

	public ProcessMessage(ISendMessage send, ILogger<ProcessMessage> logger)
	{
		_send   = send;
		_logger = logger;
	}

	public async ValueTask<bool> Get(Stop<DurableMessageProperties> parameter)
	{
		var ((identifier, _, destination, _, _), stop) = parameter;
		try
		{
			await _send.Off(parameter);
		}
		catch (OperationCanceledException) when (stop.IsCancellationRequested)
		{
			return false;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex,
							 "Failed to process distributed message for Notification ID {Id} targeting {Queue}",
							 identifier, destination);
		}

		return true;
	}
}