using DragonSpark.Compose;
using DragonSpark.Contracts.Messaging;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Runtime;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Queues.Durable;

sealed class Sweep : IStopAware
{
	readonly ChannelWriter<DurableMessageProperties> _writer;
	readonly EvaluateUnsentNotifications                 _unsent;
	readonly ILogger<Sweep>                              _logger;

	public Sweep(EvaluateUnsentNotifications unsent, ILogger<Sweep> logger)
		: this(ProcessChannel.Default, unsent, logger) {}

	public Sweep(Channel<DurableMessageProperties> channel, EvaluateUnsentNotifications unsent,
	             ILogger<Sweep> logger)
		: this(channel.Writer, unsent, logger) {}

	public Sweep(ChannelWriter<DurableMessageProperties> writer, EvaluateUnsentNotifications unsent,
	             ILogger<Sweep> logger)
	{
		_writer = writer;
		_unsent = unsent;
		_logger = logger;
	}

	public async ValueTask Get(CancellationToken parameter)
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
}