using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Materialization
{
	sealed class DefaultAny<T> : IAny<T>
	{
		public static DefaultAny<T> Default { get; } = new DefaultAny<T>();

		DefaultAny() {}

		public ValueTask<bool> Get(IQueryable<T> parameter) => parameter.AnyAsync().ToOperation();
	}
}