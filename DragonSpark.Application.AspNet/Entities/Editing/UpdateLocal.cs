namespace DragonSpark.Application.AspNet.Entities.Editing;

public sealed class UpdateLocal<T> : IModify<T> where T : class
{
	public static UpdateLocal<T> Default { get; } = new();

	UpdateLocal() {}

	public void Execute(Edit<T> parameter)
	{
		var (context, subject) = parameter;
		context.Update(subject);
	}
}