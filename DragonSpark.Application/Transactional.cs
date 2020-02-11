using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Application
{
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

		public Transactions<T> Get((Array<T> Stored, Array<T> Source) parameter)
		{
			var (first, second) = parameter;

			var stored  = first.Open();
			var source = second.Open();
			var add     = source.Except(stored, _equals).Result();
			var delete  = stored.Except(source, _equals).Result();
			var both    = stored.Union(source, _equals).ToArray();
			var update = both.Introduce(source.Union(stored, _equals).ToArray())
			                 .Select(_select)
			                 .Where(_modified)
			                 .Result();

			var result = new Transactions<T>(add, update, delete);
			return result;
		}

		sealed class Selector : ISelect<(T, T[]), (T, T)>
		{
			readonly IEqualityComparer<T> _equals;

			public Selector(IEqualityComparer<T> equals) => _equals = equals;

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

			public (T, T) Get((T, T[]) parameter) => (parameter.Item1, Other(parameter.Item2, parameter.Item1));
		}
	}
}