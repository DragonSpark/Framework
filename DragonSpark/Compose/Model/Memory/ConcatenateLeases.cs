using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences.Memory;
using System;

namespace DragonSpark.Compose.Model.Memory;

sealed class ConcatenateLeases<T> : ISelect<(Leasing<T> First, Memory<T> Second), Concatenation<T>>
{
	public static ConcatenateLeases<T> Default { get; } = new ConcatenateLeases<T>();

	ConcatenateLeases() : this(ConcatenateNewLease<T>.Default, ConcatenateExistingLease<T>.Default) {}

	readonly ConcatenateNewLease<T>      _new;
	readonly ConcatenateExistingLease<T> _existing;

	public ConcatenateLeases(ConcatenateNewLease<T> @new, ConcatenateExistingLease<T> existing)
	{
		_new      = @new;
		_existing = existing;
	}

	public Concatenation<T> Get((Leasing<T> First, Memory<T> Second) parameter)
	{
		var (first, second) = parameter;
		var size   = (uint)(first.Length + second.Length);
		var result = size <= first.ActualLength ? _existing.Get(first, second) : _new.Get(first, second, size);
		return result;
	}
}