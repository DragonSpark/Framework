using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using EFCore.BulkExtensions;
using NetFabric.Hyperlinq;
using System.Buffers;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Save;

sealed class Insert<T> : Instance<uint?>, ISave where T : class
{
	public static Insert<T> Default { get; } = new();

	Insert() : this(DefaultBatchSize.Default, DefaultWindowSize.Default) {}

	readonly ushort _notify;
	readonly uint   _batch;

	public Insert(ushort notify, uint window) : this(notify, window / 2, window) {}

	public Insert(ushort notify, uint batch, uint window) : base(window)
	{
		_notify = notify;
		_batch  = batch;
	}

	public async ValueTask<uint> Get(Stop<SaveInput> parameter)
	{
		var ((logger, size, destination, _), stop) = parameter;
		var configuration = new BulkConfig
		{
			BatchSize           = _batch.Degrade(),
			SqlBulkCopyOptions  = SqlBulkCopyOptions.KeepIdentity,
			PreserveInsertOrder = true, UseTempDB                           = false,
			NotifyAfter         = _notify.Degrade(), EnableShadowProperties = true,
			CalculateStats      = true,
		};
		var result   = 0u;
		var progress = new Progress<T>(logger, size).Execute;
		foreach (var changed in destination.ChangeTracker.Entries()
		                                   .Where(x => !x.Metadata.IsOwned())
		                                   .GroupBy(x => x.Entity.GetType()))
		{
			using var entities = changed.Select(x => x.Entity).AsValueEnumerable().ToArray(ArrayPool<object>.Shared);

			configuration.PropertiesToExclude?.Clear();
			await destination.BulkInsertAsync(entities, configuration, progress, cancellationToken: stop).Off();

			result += configuration.StatsInfo?.StatsNumberInserted is > 0 and var inserted
				          ? (uint)inserted
				          : (uint)entities.Length;
		}

		return result;
	}
}