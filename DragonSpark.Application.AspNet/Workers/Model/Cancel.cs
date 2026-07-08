using DragonSpark.Application.AspNet.Entities.Editing;

namespace DragonSpark.Application.AspNet.Workers.Model;

public sealed class Cancel : Modify<ExternalProcess>
{
	public Cancel(DefaultEdit edit) : base(edit, x => x.Enabled = false) {}
}