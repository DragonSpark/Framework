using DragonSpark.Application.AspNet.Worker.Model.Process.States;
using DragonSpark.Application.AspNet.Worker.Processes;
using DragonSpark.Contracts.Worker;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

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