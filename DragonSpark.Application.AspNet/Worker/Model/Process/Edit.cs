using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Application.AspNet.Worker.Processes;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public class Edit : EditExisting<ExternalProcess>, IEdit
{
	protected Edit(IScopes scopes, bool reload = false) : base(scopes, reload) {}
}