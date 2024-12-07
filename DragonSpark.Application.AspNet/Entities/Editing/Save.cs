namespace DragonSpark.Application.Entities.Editing;

public class Save<T> : Update<T> where T : class
{
	public Save(IEnlistedScopes scopes) : base(scopes) {}

	protected Save(IScopes scopes) : base(scopes) {}
}