namespace DragonSpark.Application.Entities.Editing;

public sealed class Save<T> : Update<T> where T : class
{
	public Save(IEnlistedScopes scopes) : base(scopes) {}
}