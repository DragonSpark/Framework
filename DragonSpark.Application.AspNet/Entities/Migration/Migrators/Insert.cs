using EFCore.BulkExtensions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Insert<T> : ISave<T> where T : class
{
	public static Insert<T> Default { get; } = new();

	Insert() {}

	public uint Get(SaveInput<T> parameter)
	{
		var (logger, size, destination, entities, total) = parameter;
		var configuration = new BulkConfig
		{
			BatchSize           = size,
			SqlBulkCopyOptions  = SqlBulkCopyOptions.KeepIdentity,
			PreserveInsertOrder = true, UseTempDB = false,
			NotifyAfter         = size,
		};

		destination.BulkInsert(entities, configuration, new Progress<T>(logger, total).Execute);
		return total;
	}
}