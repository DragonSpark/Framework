using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Materialization
{
	public sealed class SumMaterializer : IMaterializer<decimal, decimal>
	{
		public static SumMaterializer Default { get; } = new SumMaterializer();

		SumMaterializer() {}

		public ValueTask<decimal> Get(IQueryable<decimal> parameter) => parameter.SumAsync().ToOperation();
	}
}