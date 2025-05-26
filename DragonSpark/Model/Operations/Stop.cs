using System.Threading;

namespace DragonSpark.Model.Operations;

public readonly record struct Stop<T>(T Subject, CancellationToken Token)
{
	public static implicit operator T(Stop<T> instance) => instance.Subject; // TODO: 227
	public static implicit operator CancellationToken(Stop<T> instance) => instance.Token;
}