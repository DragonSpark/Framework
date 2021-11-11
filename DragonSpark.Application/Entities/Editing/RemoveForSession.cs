using DragonSpark.Compose;

namespace DragonSpark.Application.Entities.Editing;

public class RemoveForSession<T> : Modify<T> where T : class
{
	public RemoveForSession(ISessionScopes scopes) : base(scopes, RemoveLocal<T>.Default.Then().Operation()) {}
}