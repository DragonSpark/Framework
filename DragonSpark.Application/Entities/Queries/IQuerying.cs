using DragonSpark.Model.Operations;
using System.Linq;

namespace DragonSpark.Application.Entities.Queries
{
	public interface IQuerying<in T, TResult> : ISelecting<IQueryable<T>, TResult> {}
}