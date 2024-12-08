using DragonSpark.Model.Results;
using System.Linq;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Selection;

public class Query<T> : Instance<IQueryable<T>>
{
	protected Query(IQueryable<T> instance) : base(instance) {}
}