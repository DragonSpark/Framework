using DragonSpark.Compose;
using EFCore.BulkExtensions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class UpsertBatch<T> : ISaveBatch<T> where T : class
{
	public static UpsertBatch<T> Default { get; } = new();

	UpsertBatch() {}

	public uint Get(SaveBatchInput<T> parameter)
	{
		var (destination, entities) = parameter;
		var configuration = new BulkConfig
		{
			BatchSize = entities.Length, CalculateStats = true, PreserveInsertOrder = true,
		};
		destination.BulkInsertOrUpdate(entities, configuration);
		var statistics = configuration.StatsInfo.Verify();
		var result     = (uint)(statistics.StatsNumberInserted + statistics.StatsNumberUpdated);
		return result;
	}
}