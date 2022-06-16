namespace DragonSpark.Presentation.Components.Content;

public sealed class ActiveContents<T> : IActiveContents<T>
{
	public static ActiveContents<T> Default { get; } = new();

	ActiveContents() {}

	public IActiveContent<T> Get(ActiveContentInput<T> parameter) => new ActiveContent<T>(parameter.Source);
}