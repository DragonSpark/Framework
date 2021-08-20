using DragonSpark.Model.Operations;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries.Materialize
{
	public interface IMaterializer<in T, TResult> : ISelecting<IQueryable<T>, TResult> {}
}