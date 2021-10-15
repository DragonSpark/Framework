using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Compose.Model.Memory;

sealed class ConcatenateNewMemory<T> : ILease<(Memory<T> First, Memory<T> Second, uint size), T>
{
	public static ConcatenateNewMemory<T> Default { get; } = new();

	ConcatenateNewMemory() : this(NewLeasing<T>.Default) {}

	readonly INewLeasing<T> _new;

	public ConcatenateNewMemory(INewLeasing<T> @new) => _new = @new;

	public Leasing<T> Get((Memory<T> First, Memory<T> Second, uint size) parameter)
	{
		var (first, second, size) = parameter;

		var result      = _new.Get(size);
		var destination = result.AsSpan();

		var one = first.Span;
		one.CopyTo(destination[..one.Length]);
		second.Span.CopyTo(destination[one.Length..]);

		return result;
	}
}