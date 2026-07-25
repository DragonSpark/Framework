using JetBrains.Annotations;

namespace DragonSpark.Runtime;

[UsedImplicitly]
public sealed class EmptyDisposing : Disposing
{
	public static EmptyDisposing Default { get; } = new ();

	EmptyDisposing() : base(() => ValueTask.CompletedTask) {}
}