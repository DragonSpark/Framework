using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Application.AspNet.Workers.Processes;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

public class Edit : EditExisting<ExternalProcess>, IEdit
{
	protected Edit(IScopes scopes, bool reload = false) : base(scopes, reload) {}
}