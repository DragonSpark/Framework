using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using EFCore.BulkExtensions;
using NetFabric.Hyperlinq;
using System.Buffers;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Save;

sealed class Insert<T> : ISave where T : class
{
	public static Insert<T> Default { get; } = new();

	Insert() {}

	public async ValueTask<uint> Get(Stop<SaveInput> parameter)
	{
		var ((logger, size, destination, total), stop) = parameter;
		var configuration = new BulkConfig
		{
			BatchSize           = size,
			SqlBulkCopyOptions  = SqlBulkCopyOptions.KeepIdentity,
			PreserveInsertOrder = true, UseTempDB              = false,
			NotifyAfter         = size, EnableShadowProperties = true,
			CalculateStats      = true,
		};
		var result   = 0u;
		var progress = new Progress<T>(logger, total).Execute;
		foreach (var changed in destination.ChangeTracker.Entries()
		                                   .Where(x => !x.Metadata.IsOwned())
		                                   .GroupBy(x => x.Entity.GetType()))
		{
			using var entities = changed.Select(x => x.Entity).AsValueEnumerable().ToArray(ArrayPool<object>.Shared);

			configuration.PropertiesToExclude?.Clear();
			await destination.BulkInsertAsync(entities, configuration, progress, cancellationToken: stop)
			                 .Off();

			result += configuration.StatsInfo?.StatsNumberInserted is > 0 and var inserted
				          ? (uint)inserted
				          : (uint)entities.Length;
		}

		return result;
	}
}