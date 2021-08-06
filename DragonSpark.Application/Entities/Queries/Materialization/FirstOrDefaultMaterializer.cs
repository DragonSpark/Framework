using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public sealed class FirstOrDefaultMaterializer<T> : IMaterializer<T, T?>
	{
		public static FirstOrDefaultMaterializer<T> Default { get; } = new();

		FirstOrDefaultMaterializer() {}

		public async ValueTask<T?> Get(IQueryable<T> parameter)
		{
			var entity = await parameter.FirstOrDefaultAsync().ConfigureAwait(false);
			var result = entity.Account();
			return result;
		}
	}
}