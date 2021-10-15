namespace DragonSpark.Runtime;

public sealed class EmptyDisposable : Disposable
{
	public static EmptyDisposable Default { get; } = new EmptyDisposable();

	EmptyDisposable() : base(() => {}) {}
}