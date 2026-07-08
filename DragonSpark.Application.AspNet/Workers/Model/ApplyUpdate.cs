using DragonSpark.Application.AspNet.Workers.Model.States;
using DragonSpark.Contracts.Worker;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.AspNet.Workers.Model;

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