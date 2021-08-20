using DragonSpark.Application.Entities.Queries;
using System;
using System.Linq;

namespace DragonSpark.Application.Compose.Entities.Queries
{
	class Class1 {}

	public class QuerySelector<TIn, T> where T : class
	{
		readonly IQuery<TIn, T> _subject;

		public QuerySelector(IQuery<TIn, T> subject) => _subject = subject;

		public QuerySelector<TIn, TTo> Select<TTo>(Func<IQueryable<T>, IQueryable<TTo>> selection)
			where TTo : class => new(new Select<TIn, T, TTo>(_subject, selection));

		public AnyResult<TIn, T> Any() => new(_subject);

		public SingleResult<TIn, T> Single() => new(_subject);

		public SingleOrDefaultResult<TIn, T> SingleOrDefault() => new(_subject);

		public FirstResult<TIn, T> First() => new(_subject);

		public FirstOrDefaultResult<TIn, T> FirstOrDefault() => new(_subject);

		public ToArrayResult<TIn, T> ToArray() => new(_subject);

		public ToListResult<TIn, T> ToList() => new(_subject);

		public ToDictionaryResult<TIn, TKey, T> ToDictionary<TKey>(Func<T, TKey> key) where TKey : notnull
			=> new(_subject, key);

		public ToDictionaryResult<TIn, T, TKey, TValue> ToDictionary<TKey, TValue>(
			Func<T, TKey> key, Func<T, TValue> value)
			where TKey : notnull
			=> new(_subject, key, value);
	}
}