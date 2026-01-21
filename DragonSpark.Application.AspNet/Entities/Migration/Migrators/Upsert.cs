using DragonSpark.Compose;
using EFCore.BulkExtensions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Upsert<T> : ISave<T> where T : class
{
	public static Upsert<T> Default { get; } = new();

	Upsert() {}

	public uint Get(SaveInput<T> parameter)
	{
		var (logger, size, destination, entities, total) = parameter;
		var configuration = new BulkConfig
		{
			BatchSize = size, CalculateStats = true, PreserveInsertOrder = true, NotifyAfter = size,
		};
		destination.BulkInsertOrUpdate(entities, configuration, new Progress<T>(logger, total).Execute);
		var statistics = configuration.StatsInfo.Verify();
		var result     = (uint)(statistics.StatsNumberInserted + statistics.StatsNumberUpdated);
		return result;
	}
}