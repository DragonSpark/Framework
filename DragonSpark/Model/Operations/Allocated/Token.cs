using System.Threading;

namespace DragonSpark.Model.Operations.Allocated;

public readonly record struct Token<T>(T Subject, CancellationToken Item)
{
	public static implicit operator T(Token<T> instance) => instance.Subject;
}