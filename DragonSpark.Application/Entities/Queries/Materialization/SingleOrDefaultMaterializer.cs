using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public sealed class SingleOrDefaultMaterializer<T> : IMaterializer<T, T?>
	{
		public static SingleOrDefaultMaterializer<T> Default { get; } = new SingleOrDefaultMaterializer<T>();

		SingleOrDefaultMaterializer() {}

		public async ValueTask<T?> Get(IQueryable<T> parameter)
		{
			var entity = await parameter.SingleOrDefaultAsync().ConfigureAwait(false);
			var result = entity.Account();
			return result;
		}
	}
}