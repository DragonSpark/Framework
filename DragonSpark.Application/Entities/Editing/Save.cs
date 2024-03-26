namespace DragonSpark.Application.Entities.Editing;

public class Save<T> : Update<T> where T : class
{
	public Save(IEnlistedContexts contexts) : base(contexts) {}

	protected Save(IContexts contexts) : base(contexts) {}
}