using System.Threading;

namespace DragonSpark.Model.Operations;

public readonly record struct Stop<T>(T Subject, CancellationToken Token)
{
	public static implicit operator T(Stop<T> instance) => instance.Subject; // TODO: Stop: Audit
	public static implicit operator CancellationToken(Stop<T> instance) => instance.Token;

	public Stop(T Subject) : this(Subject, AmbientToken.Default) {}
}