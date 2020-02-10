using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application
{
	public interface ITransactional<T> : ISelect<(Array<T> Stored, Array<T> Updated), Transactions<T>> {}

	public class Transactional<T> : ITransactional<T> where T : class
	{
		readonly IEqualityComparer<T>   _equals;
		readonly Func<(T, T), bool>     _modified;
		readonly Func<(T, T[]), (T, T)> _select;

		public Transactional(IEqualityComparer<T> equals, Func<(T, T), bool> modified)
			: this(equals, modified, new Selector(equals).Get) {}

		public Transactional(IEqualityComparer<T> equals, Func<(T, T), bool> modified, Func<(T, T[]), (T, T)> select)
		{
			_equals   = equals;
			_modified = modified;
			_select   = select;
		}

		sealed class Selector : ISelect<(T, T[]), (T, T)>
		{
			readonly IEqualityComparer<T> _equals;

			public Selector(IEqualityComparer<T> equals) => _equals = @equals;

			public (T, T) Get((T, T[]) parameter) => (parameter.Item1, Other(parameter.Item2, parameter.Item1));

			T Other(T[] source, T identity)
			{
				var length = source.Length;
				for (var i = 0u; i < length; i++)
				{
					var current = source[i];
					if (_equals.Equals(current, identity))
					{
						return current;
					}
				}

				throw new InvalidOperationException("Element not found.");
			}
		}

		public Transactions<T> Get((Array<T> Stored, Array<T> Updated) parameter)
		{
			var (existing, input) = parameter;

			var stored  = existing.Open();
			var updated = input.Open();
			var add     = updated.Except(stored, _equals).Result();
			var delete  = stored.Except(updated, _equals).Result();
			var both    = stored.Union(updated, _equals).ToArray();
			var update = both.Introduce(updated.Union(stored, _equals).ToArray())
			                 .Select(_select)
			                 .Where(_modified)
			                 .Result();

			var result = new Transactions<T>(add, update, delete);
			return result;
		}
	}

	public readonly struct Transactions<T>
	{
		public Transactions(Array<T> add, Array<(T Stored, T Current)> update, Array<T> delete)
		{
			Add    = add;
			Update = update;
			Delete = delete;
		}

		public Array<T> Add { get; }

		public Array<(T Stored, T Current)> Update { get; }

		public Array<T> Delete { get; }

		public bool Any() => Add.Length > 0 || Update.Length > 0 || Delete.Length > 0;
	}
}