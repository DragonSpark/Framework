namespace DragonSpark.Application.Compose.Entities.Queries
{
	class Class1 {}

	/*public class QuerySelector<T> where T : class
	{
		readonly IQuery<T> _subject;

		public QuerySelector(IQuery<T> subject) => _subject = subject;

		public QuerySelector<TIn, T> Accept<TIn>() => new(new Accept<TIn, T>(_subject));

		public QuerySelector<T> Where(Expression<Func<T, bool>> select) => Next(new WhereSelector<None, T>(select));

		public QuerySelector<T> OrderBy<TProperty>(Expression<Func<T, TProperty>> property)
			=> Select(x => x.OrderBy(property));

		public QuerySelector<T> OrderByDescending<TProperty>(Expression<Func<T, TProperty>> property)
			=> Select(x => x.OrderByDescending(property));

		public QuerySelector<TOther> OfType<TOther>() where TOther : class, T => Select(x => x.OfType<TOther>());

		public QuerySelector<TTo> Select<TTo>(Func<IQueryable<T>, IQueryable<TTo>> select) where TTo : class
			=> Next(new SelectionSelector<None, T, TTo>(select));

		public QuerySelector<TTo> Select<TTo>(Expression<Func<T, TTo>> select) where TTo : class
			=> Next(new SelectSelector<None, T, TTo>(select));

		public QuerySelector<TTo> SelectMany<TTo>(Expression<Func<T, IEnumerable<TTo>>> select) where TTo : class
			=> Next(new SelectManySelector<None, T, TTo>(select));

		QuerySelector<TTo> Next<TTo>(ISelector<None, T, TTo> @select) where TTo : class
			=> new(new Adapter<T, TTo>(_subject, select));

		public IResulting<bool> Any() => Accept<None>().Any().Then().Bind(None.Default).Out();

		public IResulting<T> Single() => Accept<None>().Single().Then().Bind(None.Default).Out();

		public IResulting<T?> SingleOrDefault() => Accept<None>().SingleOrDefault().Then().Bind(None.Default).Out();

		public IResulting<T> First() => Accept<None>().First().Then().Bind(None.Default).Out();

		public IResulting<T?> FirstOrDefault() => Accept<None>().FirstOrDefault().Then().Bind(None.Default).Out();

		public IResulting<Array<T>> ToArray() => Accept<None>().ToArray().Then().Bind(None.Default).Out();

		public IResulting<List<T>> ToList() => Accept<None>().ToList().Then().Bind(None.Default).Out();

		public IResulting<IReadOnlyDictionary<TKey, T>> ToDictionary<TKey>(Func<T, TKey> key) where TKey : notnull
			=> Accept<None>().ToDictionary(key).Then().Bind(None.Default).Out();

		public IResulting<IReadOnlyDictionary<TKey, TValue>> ToDictionary<TKey, TValue>(
			Func<T, TKey> key, Func<T, TValue> value)
			where TKey : notnull
			=> Accept<None>().ToDictionary(key, value).Then().Bind(None.Default).Out();
	}

	public class QuerySelector<TIn, T> where T : class
	{
		readonly IQuery<TIn, T> _subject;

		public QuerySelector(IQuery<TIn, T> subject) => _subject = subject;

		public QuerySelector<TIn, T> Where(Expression<Func<T, bool>> select) => Next(new WhereSelector<TIn, T>(select));

		public QuerySelector<TIn, T> Where(Express<TIn, T> select) => Next(new WhereSelector<TIn, T>(select));

		public QuerySelector<TIn, TTo> Select<TTo>(Expression<Func<T, TTo>> select) where TTo : class
			=> Next(new SelectSelector<TIn, T, TTo>(@select));

		public QuerySelector<TIn, TTo> Select<TTo>(Express<TIn, T, TTo> select) where TTo : class
			=> Next(new SelectSelector<TIn, T, TTo>(@select));

		public QuerySelector<TIn, TTo> SelectMany<TTo>(Expression<Func<T, IEnumerable<TTo>>> select) where TTo : class
			=> Next(new SelectManySelector<TIn, T, TTo>(@select));

		public QuerySelector<TIn, TTo> SelectMany<TTo>(Express<TIn, T, IEnumerable<TTo>> select) where TTo : class
			=> Next(new SelectManySelector<TIn, T, TTo>(@select));

		public QuerySelector<TIn, TTo> Select<TTo>(Func<IQueryable<T>, IQueryable<TTo>> selection)
			where TTo : class
			=> Next(new SelectionSelector<TIn, T, TTo>(selection));

		public QuerySelector<TIn, TTo> Select<TTo>(Func<TIn, Func<IQueryable<T>, IQueryable<TTo>>> selection)
			where TTo : class
			=> Next(new SelectionSelector<TIn, T, TTo>(selection));

		QuerySelector<TIn, TTo> Next<TTo>(ISelector<TIn, T, TTo> @select) where TTo : class
			=> new(new Query<TIn, T, TTo>(_subject, select));

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
	}*/
}