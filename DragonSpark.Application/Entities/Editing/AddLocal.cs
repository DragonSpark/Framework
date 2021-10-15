namespace DragonSpark.Application.Entities.Editing;

public sealed class AddLocal<T> : IModify<T> where T : class
{
	public static AddLocal<T> Default { get; } = new ();

	AddLocal() {}

	public void Execute(Edit<T> parameter)
	{
		var (context, subject) = parameter;
		context.Add(subject);
	}
}