using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Compose;
using DragonSpark.Contracts.Worker;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

public class StatusAwareProcess<T> : IStopAware<T> where T : ExternalProcess
{
	readonly IStopAware<T> _previous;
	readonly Log           _log;

	protected StatusAwareProcess(IStopAware<T> previous, Log log)
	{
		_previous = previous;
		_log      = log;
	}

	public ValueTask Get(Stop<T> parameter)
	{
		var (subject, _) = parameter;
		var latest = subject.Updates.MaxBy(x => x.Created)?.Status ?? ProcessStatus.Queued;

		switch (latest)
		{
			case ProcessStatus.New:
			case ProcessStatus.Error:
			case ProcessStatus.Queued:
			case ProcessStatus.Paused:
				return _previous.Get(parameter);
		}

		_log.Execute(subject.Id, latest);
		throw new InvalidOperationException("Invalid status detected.");
	}

	public sealed class Log : LogError<Guid, ProcessStatus>
	{
		public Log(ILogger<Log> logger)
			: base(logger, $"{A.Type<T>().Name} '{{Identity}}' has an unexpected status of '{{Status}}'") {}
	}
}