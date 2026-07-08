using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Contracts.Worker;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

sealed class RelayError<T> : StopAware<T> where T : ExternalProcess
{
	public RelayError(IEdit edit, string message = "A problem occurred while processing.")
		: base(new Relay<T>(new AppendState(ProcessStatus.Error, message, edit))) {}
}