using DragonSpark.Compose;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class Sweep : ISweep
{
	readonly ChannelWriter<DurableMessageProperties> _writer;
	readonly EvaluateUnsentNotifications             _unsent;
	readonly ILogger<Sweep>                          _logger;

	public Sweep(ChannelWriter<DurableMessageProperties> writer, EvaluateUnsentNotifications unsent,
	             ILogger<Sweep> logger)
	{
		_writer = writer;
		_unsent = unsent;
		_logger = logger;
	}

	public async ValueTask<bool> Get(Stop<None> parameter)
	{
		try
		{
			using var unsent = await _unsent.Off(new(Time.Default, parameter));
			foreach (var message in unsent)
			{
				if (!_writer.TryWrite(message))
				{
					_logger.LogWarning("ProcessChannel capacity reached while sweeping Notification ID {Id}",
					                   message.Identifier);
					break;
				}
			}
		}
		catch (OperationCanceledException) when (parameter.Token.IsCancellationRequested)
		{
			return false;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "An error occurred during outbox database sweep");
		}

		return true;
	}
}