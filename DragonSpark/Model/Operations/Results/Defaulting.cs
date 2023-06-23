namespace DragonSpark.Model.Operations.Results;

public sealed class Defaulting<T> : Instance<T?>
{
	public static Defaulting<T> Default { get; } = new();

	Defaulting() : base(default) {}
}