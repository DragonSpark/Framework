using DragonSpark.Compose;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Queries.Runtime.Materialize;

public sealed class SingleMaterializer<T> : IMaterializer<T, T>
{
	public static SingleMaterializer<T> Default { get; } = new();

	SingleMaterializer() {}

	public ValueTask<T> Get(IQueryable<T> parameter) => parameter.SingleAsync().ToOperation();
}