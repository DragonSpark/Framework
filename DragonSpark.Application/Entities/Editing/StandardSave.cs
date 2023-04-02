namespace DragonSpark.Application.Entities.Editing;

public class StandardSave<T> : Save<T> where T : class
{
	public StandardSave(IStandardScopes scopes) : base(scopes) {}
}