using DragonSpark.Model.Sequences;

namespace DragonSpark.Presentation.Model;

public static class Options
{
	public static Array<Option<T>> For<T>() where T : struct, Enum => EnumerationOptions<T>.Default.Get();
}

public class Options<T> : Instances<Option<T>>
{
	protected Options(IEnumerable<Option<T>> enumerable) : base(enumerable) {}

	public Options(params Option<T>[] instance) : base(instance) {}
}