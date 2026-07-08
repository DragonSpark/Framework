using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Editing;

namespace DragonSpark.Application.AspNet.Workers.Model;

public class Edit : EditExisting<ExternalProcess>, IEdit
{
	protected Edit(IScopes scopes, bool reload = false) : base(scopes, reload) {}
}