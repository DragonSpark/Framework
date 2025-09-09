using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.Runtime.Operations;

public sealed class EmptyTokenHandle : ITokenHandle
{
	public static EmptyTokenHandle Default { get; } = new();

	EmptyTokenHandle() : this(CancellationToken.None) {}

	public EmptyTokenHandle(CancellationToken token) => Token = token;

	public ValueTask Get() => ValueTask.CompletedTask;

	public CancellationToken Token { get; }
}