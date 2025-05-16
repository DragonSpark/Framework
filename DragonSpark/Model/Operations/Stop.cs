using System.Threading;

namespace DragonSpark.Model.Operations;

public readonly record struct Stop<T>(T Subject, CancellationToken Token = default)
{
	public static implicit operator T(Stop<T> instance) => instance.Subject;

	public static implicit operator CancellationToken(Stop<T> instance) => instance.Token;
}