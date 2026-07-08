using DragonSpark.Application.AspNet.Workers.Model.Process.States;
using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Contracts.Worker;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

sealed class ApplyUpdate : ICommand<ExternalProcess>
{
	readonly IUpdate _update;

	public ApplyUpdate(ProcessStatus status, string? message) : this(new Update(status, message)) {}

	public ApplyUpdate(IUpdate update) => _update = update;

	public void Execute(ExternalProcess parameter)
	{
		var update = _update.Get();
		parameter.Update(update);
	}
}