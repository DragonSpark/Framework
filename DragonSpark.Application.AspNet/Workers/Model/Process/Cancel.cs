using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Application.AspNet.Workers.Processes;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

public sealed class Cancel : Modify<ExternalProcess>
{
	public Cancel(DefaultEdit edit) : base(edit, x => x.Enabled = false) {}
}