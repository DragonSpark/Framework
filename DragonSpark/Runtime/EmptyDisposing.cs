using System.Threading.Tasks;

namespace DragonSpark.Runtime;

public sealed class EmptyDisposing : Disposing
{
	public static EmptyDisposing Default { get; } = new ();

	EmptyDisposing() : base(() => ValueTask.CompletedTask) {}
}