using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Compose.Model.Memory;

sealed class ConcatenateNewLease<T> : ISelect<(Leasing<T> First, Memory<T> Second, uint size), Concatenation<T>>
{
	public static ConcatenateNewLease<T> Default { get; } = new();

	ConcatenateNewLease() : this(NewLeasing<T>.Default) {}

	readonly INewLeasing<T> _new;

	public ConcatenateNewLease(INewLeasing<T> @new) => _new = @new;

	public Concatenation<T> Get((Leasing<T> First, Memory<T> Second, uint size) parameter)
	{
		var (first, second, size) = parameter;

		var result      = _new.Get(size);
		var destination = result.AsSpan();

		var one = first.AsSpan();
		one.CopyTo(destination[..one.Length]);
		second.Span.CopyTo(destination[one.Length..]);

		return new (first, result);
	}
}