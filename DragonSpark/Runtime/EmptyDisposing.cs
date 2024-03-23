using JetBrains.Annotations;
using System.Threading.Tasks;

namespace DragonSpark.Runtime;

[UsedImplicitly]
public sealed class EmptyDisposing : Disposing
{
	public static EmptyDisposing Default { get; } = new ();

	EmptyDisposing() : base(() => ValueTask.CompletedTask) {}
}