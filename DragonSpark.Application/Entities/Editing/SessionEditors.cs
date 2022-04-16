using DragonSpark.Model;

namespace DragonSpark.Application.Entities.Editing;

public sealed class SessionEditors : Editors<None>
{
	public SessionEditors(ISessionScopes scopes) : base(scopes) {}
}