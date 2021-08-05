using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using System;
using System.Collections.Generic;

namespace DragonSpark.Application.Runtime
{
	public class Transactional<T> : ITransactional<T> where T : class
	{
		readonly IEqualityComparer<T>         _equals;
		readonly Func<(T, T), bool>           _modified;
		readonly Func<(T, Memory<T>), (T, T)> _select;

		public Transactional(IEqualityComparer<T> equals, Func<(T, T), bool> modified)
			: this(equals, modified, new Selector(equals).Get) {}

		public Transactional(IEqualityComparer<T> equals, Func<(T, T), bool> modified,
		                     Func<(T, Memory<T>), (T, T)> select)
		{
			_equals   = equals;
			_modified = modified;
			_select   = select;
		}

		public Transactions<T> Get((Memory<T> Stored, Memory<T> Source) parameter)
		{
			var (first, second) = parameter;
			var (left, add)     = Left(first, second);
			var (right, delete) = Right(first, second);

			var       update = ArrayBuilder.New<(T, T)>(right.Count);
			using var lease  = left.AsLease();
			var       both   = lease.AsMemory();
			foreach (var element in right.AsSpan())
			{
				var select = _select((element, both));
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

		readonly struct State : IDisposable
		{
			public State(ArrayBuilder<T> same, ArrayBuilder<T> difference)
			{
				Same       = same;
				Difference = difference;
			}

			public ArrayBuilder<T> Same { get; }

			public ArrayBuilder<T> Difference { get; }

			public void Deconstruct(out ArrayBuilder<T> same, out ArrayBuilder<T> difference)
			{
				same       = Same;
				difference = Difference;
			}

			public void Dispose()
			{
				Same.Dispose();
				Difference.Dispose();
			}
		}

		sealed class Selector : ISelect<(T, Memory<T>), (T, T)>
		{
			readonly IEqualityComparer<T> _equals;

			public Selector(IEqualityComparer<T> equals) => _equals = equals;

			public (T, T) Get((T, Memory<T>) parameter) => (parameter.Item1, Other(parameter.Item2, parameter.Item1));

			T Other(Memory<T> source, T identity)
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
}