using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public sealed class EmptyStopHandle : IStopHandle
{
	public static EmptyStopHandle Default { get; } = new();

	EmptyStopHandle() : this(CancellationToken.None) {}

	public EmptyStopHandle(CancellationToken token) => Token = token;

	public ValueTask Get() => ValueTask.CompletedTask;

	public CancellationToken Token { get; }
}