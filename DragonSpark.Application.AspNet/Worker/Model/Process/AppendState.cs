using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Application.AspNet.Worker.Model.Process.States;
using DragonSpark.Application.AspNet.Worker.Processes;
using DragonSpark.Contracts.Worker;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

sealed class AppendState : Modify<ExternalProcess>
{
	public AppendState(ProcessStatus status, string? message, IEdit<ExternalProcess> edit)
		: this(new Update(status, message), edit) {}

	public AppendState(IUpdate update, IEdit<ExternalProcess> edit) : base(edit, new ApplyUpdate(update)) {}
}