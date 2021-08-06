using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public sealed class FirstMaterializer<T> : IMaterializer<T, T>
	{
		public static FirstMaterializer<T> Default { get; } = new();

		FirstMaterializer() {}

		public ValueTask<T> Get(IQueryable<T> parameter) => parameter.FirstAsync().ToOperation();
	}
}