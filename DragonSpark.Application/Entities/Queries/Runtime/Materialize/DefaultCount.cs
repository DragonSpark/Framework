using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

sealed class DefaultCount<T> : ICount<T>
{
	public static DefaultCount<T> Default { get; } = new DefaultCount<T>();

	DefaultCount() {}

	public async ValueTask<uint> Get(IQueryable<T> parameter)
	{
		var count  = await parameter.CountAsync().ConfigureAwait(false);
		var result = count.Grade();
		return result;
	}
}