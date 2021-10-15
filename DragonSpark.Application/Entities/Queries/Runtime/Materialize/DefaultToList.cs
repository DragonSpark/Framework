using DragonSpark.Compose;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

sealed class DefaultToList<T> : IToList<T>
{
	public static DefaultToList<T> Default { get; } = new DefaultToList<T>();

	DefaultToList() {}

	public ValueTask<List<T>> Get(IQueryable<T> parameter) => parameter.ToList().ToOperation();
}