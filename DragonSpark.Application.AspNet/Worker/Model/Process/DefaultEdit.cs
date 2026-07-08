using DragonSpark.Application.AspNet.Entities;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public sealed class DefaultEdit : Edit
{
	public DefaultEdit(IScopes scopes) : base(scopes) {}
}