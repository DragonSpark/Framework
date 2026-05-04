using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using NetFabric.Hyperlinq;
using System.Buffers;
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
		var ((logger, size, destination, entities, total), stop) = parameter;
		var configuration = new BulkConfig
		{
			BatchSize              = size, CalculateStats = true, PreserveInsertOrder = true, NotifyAfter = size,
			EnableShadowProperties = true
		};
		var progress = new Progress<T>(logger, total).Execute;
		await foreach (var chunk in entities.AsAsyncEnumerable().Chunk(size * _factor).WithCancellation(stop))
		{
			using var page = chunk.AsValueEnumerable().ToArray(ArrayPool<T>.Shared);

			var entries = destination.ChangeTracker.Entries();
			foreach (var changed in entries.Where(x => !x.Metadata.IsOwned()).GroupBy(x => x.Entity.GetType()))
			{
				await destination.BulkInsertOrUpdateAsync(changed.Select(x => x.Entity), configuration, progress,
				                                          cancellationToken: stop)
				                 .Off();				
			}

			destination.ChangeTracker.Clear();
		}

		var statistics = configuration.StatsInfo.Verify();
		var result     = (uint)(statistics.StatsNumberInserted + statistics.StatsNumberUpdated);
		return result;
	}
}