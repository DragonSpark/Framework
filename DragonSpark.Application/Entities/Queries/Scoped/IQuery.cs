using DragonSpark.Model.Selection;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Scoped
{
	public interface IQuery<in TKey, out T> : ISelect<TKey, IQueryable<T>> {}
}