using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.Hosting.Server.GitHub
{
	public sealed class LoggedProcessorOperation<T> : Instance<IOperation<EventMessage>>
	{
		[UsedImplicitly]
		public LoggedProcessorOperation(IProcessor processor, TaskInformationTemplate<T> information,
		                                FaultAwareTemplate<T> fault)
			: base(processor.Then()
			                .Bind(information)
			                .WithArguments(x => (x.Parameter.Header.Delivery,
			                                     x.Parameter.Header.Event,
			                                     x.Task.IsCompleted))
			                .Then()
			                .Bind(fault)
			                .WithArguments(x => (x.Header.Delivery, x.Header.Event))) {}
	}

	public sealed class TaskInformationTemplate<T> : LogDetailed<string, string, bool>
	{
		public TaskInformationTemplate(ILogger<T> logger)
			: base(logger, "[{Id}] '{Type}' Completed: {Completed}.") {}
	}

	public sealed class FaultAwareTemplate<T> : LogDetailedException<string, string>
	{
		public FaultAwareTemplate(ILogger<T> logger)
			: base(logger,
			       "[{Id}] An exception occurred while processing an event message of type '{MessageType}'.") {}
	}
}