using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Runtime;

public class Transactional<T> : ITransactional<T> where T : class
{
	readonly IEqualityComparer<T>          _equals;
	readonly Func<Mapping<T>, bool>        _modified;
	readonly Func<Elements<T>, Mapping<T>> _select;

	protected Transactional() : this(_ => false) {}

	protected Transactional(Func<Mapping<T>, bool> modified) : this(EqualityComparer<T>.Default, modified) {}

	protected Transactional(IEqualityComparer<T> equals, Func<Mapping<T>, bool> modified)
		: this(equals, modified, new Selector(equals).Get) {}

	protected Transactional(IEqualityComparer<T> equals, Func<Mapping<T>, bool> modified,
	                        Func<Elements<T>, Mapping<T>> select)
	{
		_equals   = equals;
		_modified = modified;
		_select   = select;
	}

	public Transactions<T> Get(TransactionInput<T> parameter)
	{
		var (first, second) = parameter;
		var (left, add)     = Left(first, second);
		var (right, delete) = Right(first, second);

		using var update = ArrayBuilder.New<Mapping<T>>(right.Count);
		using var lease  = left.AsLease();
		var       both   = lease.AsMemory();
		foreach (var element in right.AsSpan())
		{
			var select = _select(new (element, both));
			if (_modified(select))
			{
				update.UncheckedAdd(select);
			}
		}

		right.Dispose();

		return new(add.AsLease(), update.AsLease(), delete.AsLease());
	}

	State Left(Memory<T> first, Memory<T> second)
	{
		var add  = ArrayBuilder.New<T>(second.Length);
		var left = ArrayBuilder.New<T>(second.Length);
		var span = second.Span;
		for (var i = 0; i < second.Length; i++)
		{
			var candidate = span[i];
			if (first.Then().Contains(candidate, _equals))
			{
				left.UncheckedAdd(candidate);
			}
			else
			{
				add.UncheckedAdd(candidate);
			}
		}

		return new(left, add);
	}

	State Right(Memory<T> first, Memory<T> second)
	{
		var delete = ArrayBuilder.New<T>(first.Length);
		var right  = ArrayBuilder.New<T>(first.Length);
		var span   = first.Span;
		for (var i = 0; i < first.Length; i++)
		{
			var candidate = span[i];
			if (second.Then().Contains(candidate, _equals))
			{
				right.UncheckedAdd(candidate);
			}
			else
			{
				delete.UncheckedAdd(candidate);
			}
		}

		return new(right, delete);
	}

	readonly record struct State(ArrayBuilder<T> Same, ArrayBuilder<T> Difference) : IDisposable
	{
		public void Dispose()
		{
			Same.Dispose();
			Difference.Dispose();
		}
	}

	sealed class Selector : ISelect<Elements<T>, Mapping<T>>
	{
		readonly IEqualityComparer<T> _equals;

		public Selector(IEqualityComparer<T> equals) => _equals = equals;

		public Mapping<T> Get(Elements<T> parameter)
			=> new (parameter.Source, Destination(parameter.Candidates, parameter.Source));

		T Destination(Memory<T> source, T identity)
		{
			var length = source.Length;
			for (var i = 0; i < length; i++)
			{
				var current = source.Span[i];
				if (_equals.Equals(current, identity))
				{
					return current;
				}
			}

			throw new InvalidOperationException("Element not found.");
		}
	}
}

// TODO

public readonly record struct Elements<T>(T Source, Memory<T> Candidates);

public readonly record struct Mapping<T>(T Stored, T Destination);