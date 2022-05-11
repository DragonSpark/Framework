using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Model.Sequences;

public class Transactional<T> : ITransactional<T> where T : class
{
	readonly IEqualityComparer<T>         _equals;
	readonly Func<Update<T>, bool>        _modified;
	readonly Func<Location<T>, Update<T>> _select;

	protected Transactional() : this(_ => false) { }

	protected Transactional(Func<Update<T>, bool> modified) : this(EqualityComparer<T>.Default, modified) { }

	protected Transactional(IEqualityComparer<T> equals, Func<Update<T>, bool> modified)
		: this(equals, modified, new Selector(equals).Get) { }

	protected Transactional(IEqualityComparer<T> equals, Func<Update<T>, bool> modified,
							Func<Location<T>, Update<T>> select)
	{
		_equals = equals;
		_modified = modified;
		_select = select;
	}

	public Transactions<T> Get(TransactionInput<T> parameter)
	{
		var (first, second) = parameter;
		var (left, add) = Left(first, second);
		var (sources, delete) = Right(first, second);

		using var update = ArrayBuilder.New<Update<T>>(sources.Count);
		using var lease = left.AsLease();
		var inputs = lease.AsMemory();
		foreach (var stored in sources.AsSpan())
		{
			var select = _select(new(inputs, stored));
			if (_modified(select))
			{
				update.UncheckedAdd(select);
			}
		}

		sources.Dispose();

		return new(add.AsLease(), update.AsLease(), delete.AsLease());
	}

	State Left(Memory<T> first, Memory<T> second)
	{
		var count = second.Length;
		var add = ArrayBuilder.New<T>(count);
		var left = ArrayBuilder.New<T>(count);
		var span = second.Span;
		var selector = first.Then();
		for (var i = 0; i < count; i++)
		{
			var candidate = span[i];
			if (selector.Contains(candidate, _equals))
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
		var right = ArrayBuilder.New<T>(first.Length);
		var span = first.Span;
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

	sealed class Selector : ISelect<Location<T>, Update<T>>
	{
		readonly IEqualityComparer<T> _equals;

		public Selector(IEqualityComparer<T> equals) => _equals = equals;

		public Update<T> Get(Location<T> parameter)
			=> new(parameter.Stored, Input(parameter.Inputs, parameter.Stored));

		T Input(Memory<T> source, T identity)
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