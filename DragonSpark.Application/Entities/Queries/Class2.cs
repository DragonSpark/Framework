using DragonSpark.Model.Selection;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries
{
	class Class2 {}

	public readonly struct In<T, TKey>
	{
		public In(IQueryable<T> query, TKey parameter)
		{
			Query     = query;
			Parameter = parameter;
		}

		public IQueryable<T> Query { get; }

		public TKey Parameter { get; }

		public void Deconstruct(out IQueryable<T> query, out TKey parameter)
		{
			query     = Query;
			parameter = Parameter;
		}
	}

	public interface ISelector<TKey, TOut> : ISelector<TOut, TKey, TOut> {}

	public interface ISelector<T, TKey, out TOut> : ISelect<In<T, TKey>, IQueryable<TOut>> {}

	public class WhereSelector<TKey, T> : ISelector<TKey, T>
	{
		readonly Express<TKey, T> _select;

		public WhereSelector(Express<TKey, T> select) => _select = select;

		public IQueryable<T> Get(In<T, TKey> parameter) => parameter.Query.Where(_select(parameter.Parameter));
	}
}