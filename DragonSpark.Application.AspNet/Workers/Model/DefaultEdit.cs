using DragonSpark.Application.AspNet.Entities;

namespace DragonSpark.Application.AspNet.Workers.Model;

public sealed class DefaultEdit : Edit
{
	public DefaultEdit(IScopes scopes) : base(scopes) {}
}