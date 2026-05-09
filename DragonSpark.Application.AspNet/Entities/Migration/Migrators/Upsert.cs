using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Upsert<T> : ISave<T> where T : class
{
	public static Upsert<T> Default { get; } = new();

	Upsert() : this(DefaultChunkFactor.Default) {}

	readonly byte _factor;

	public Upsert(byte factor) => _factor = factor;

	public async ValueTask<uint> Get(Stop<SaveInput<T>> parameter)
	{
		var ((_, size, destination, entities, _), stop) = parameter;

		var             result      = 0u;
		await using var transaction = await destination.Database.BeginTransactionAsync(stop).Off();
		await foreach (var _ in entities.AsAsyncEnumerable().Chunk(size * _factor).WithCancellation(stop))
		{
			result += (uint)await destination.SaveChangesAsync().Off();
			destination.ChangeTracker.Clear();
		}
		await transaction.CommitAsync().Off();

		return result;
	}
}