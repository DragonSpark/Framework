using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Application.AspNet.Worker.Processes;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public sealed class Cancel : Modify<ExternalProcess>
{
	public Cancel(DefaultEdit edit) : base(edit, x => x.Enabled = false) {}
}