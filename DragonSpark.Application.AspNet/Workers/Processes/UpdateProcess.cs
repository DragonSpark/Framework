using DragonSpark.Compose;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.AspNet.Workers.Processes;

sealed class UpdateProcess : ICommand<UpdateProcessInput>
{
	public static UpdateProcess Default { get; } = new();

	UpdateProcess() {}

	public void Execute(UpdateProcessInput parameter)
	{
		var (process, update) = parameter;
		process.Updates       = process.Updates.Account() ?? [];
		process.Updates.Add(update);
		process.State = new(update.Created, update.Message, update.Status);

	}
}