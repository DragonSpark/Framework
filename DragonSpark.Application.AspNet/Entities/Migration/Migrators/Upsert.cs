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

	public Upsert(byte factor) => _factor   = factor;

	public async ValueTask<uint> Get(Stop<SaveInput<T>> parameter)
	{
		var ((_, size, destination, entities, _), stop) = parameter;
		
		/*var configuration = new BulkConfig
		{
			BatchSize         = size, CalculateStats          = true, PreserveInsertOrder = true, NotifyAfter = size,
			SetOutputIdentity = false, EnableShadowProperties = true
		};
		var progress = new Progress<T>(logger, total).Execute;*/
		/*// Temporarily remove concurrency token from the model (for this DbContext only)
		var rowVersionProp = destination.Model.FindEntityType(typeof(YourEntity))
		                            ?.FindProperty(nameof(YourEntity.RowVersion));

		if (rowVersionProp != null)
		{
			rowVersionProp.SetIsConcurrencyToken(false);
		}*/
		var result = 0u;
		await foreach (var _ in entities.AsAsyncEnumerable().Chunk(size * _factor).WithCancellation(stop))
		{
			//await using var transaction = await destination.Database.BeginTransactionAsync(stop).Off();
			result += (uint)await destination.SaveChangesAsync().Off();
			/*using var       page        = chunk.AsValueEnumerable().ToArray(ArrayPool<T>.Shared);

			foreach (var changed in destination.ChangeTracker.Entries()
			                                   .Where(x => !x.Metadata.IsOwned())
			                                   .GroupBy(x => x.Metadata))
			{
				try
				{
					var type = changed.Key.ClrType;
					var target = type == typeof(T);
					configuration.SetOutputIdentity = _identity.Get(changed.Key) && !target;
					var select = changed.Select(x => x.Entity).ToList(); // TODO V2
					await destination.BulkInsertOrUpdateAsync(select, configuration, progress, type,
					                                          cancellationToken: stop).Off();
					/*await destination.BulkInsertOrUpdateAsync(select, configuration, progress, cancellationToken: stop)
					                 .Off();#1#
				}
				catch (Exception e)
				{
					throw;
				}
			}*/

//			await transaction.CommitAsync().Off();

			destination.ChangeTracker.Clear();
		}

		/*var statistics = configuration.StatsInfo.Verify();
		var result     = (uint)(statistics.StatsNumberInserted + statistics.StatsNumberUpdated);*/
		return result;
	}
}