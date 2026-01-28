using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using EFCore.BulkExtensions;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Upsert<T> : ISave<T> where T : class
{
	public static Upsert<T> Default { get; } = new();

	Upsert() {}

	public async ValueTask<uint> Get(Stop<SaveInput<T>> parameter)
	{
		var ((logger, size, destination, entities, total), stop) = parameter;
		var configuration = new BulkConfig
		{
			BatchSize = size, CalculateStats = true, PreserveInsertOrder = true, NotifyAfter = size,
		};
		await destination.BulkInsertOrUpdateAsync(entities, configuration, new Progress<T>(logger, total).Execute,
		                                          cancellationToken: stop)
		                 .Off();
		var statistics = configuration.StatsInfo.Verify();
		var result     = (uint)(statistics.StatsNumberInserted + statistics.StatsNumberUpdated);
		return result;
	}
}