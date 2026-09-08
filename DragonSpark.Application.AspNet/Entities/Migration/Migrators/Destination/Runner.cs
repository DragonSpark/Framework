using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Destination;

sealed class Runner<T> : IRunner<T>
{
	readonly IResult<Channel<DbContext>> _channel;
	readonly IWriter<T>                  _writer;

	public Runner(IWriter<T> writer) : this(DestinationChannel<DbContext>.Default, writer) {}

	public Runner(IResult<Channel<DbContext>> channel, IWriter<T> writer)
	{
		_channel = channel;
		_writer  = writer;
	}

	public RunnerResult Get(Stop<DestinationInput<T>> parameter)
	{
		var ((_, source, workspaces, from, _), stop) = parameter;

		var channel = _channel.Get();
		var work    = _writer.Get(new(new(source, workspaces, from, channel.Writer), stop));
		return new(work, channel.Reader);
	}
}