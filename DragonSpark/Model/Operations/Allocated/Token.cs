using System.Threading;

namespace DragonSpark.Model.Operations.Allocated;

public readonly record struct Token<T>(T Subject, CancellationToken Item = default)
{
	public static implicit operator T(Token<T> instance) => instance.Subject;

	public static implicit operator CancellationToken(Token<T> instance) => instance.Item;
}