using DragonSpark.Compose;
using EFCore.BulkExtensions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Update<T> : ISave<T> where T : class
{
	public static Update<T> Default { get; } = new();

	Update() {}

	public uint Get(SaveInput<T> parameter)
	{
		var (logger, size, destination, entities, total) = parameter;
		var configuration = new BulkConfig { BatchSize = size, CalculateStats = true, NotifyAfter = size };
		destination.BulkUpdate(entities, configuration, new Progress<T>(logger, total).Execute);
		var result = configuration.StatsInfo.Verify().StatsNumberUpdated.Grade();
		return result;
	}
}