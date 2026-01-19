using EFCore.BulkExtensions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class InsertBatch<T> : ISaveBatch<T> where T : class
{
	public static InsertBatch<T> Default { get; } = new();

	InsertBatch() {}

	public uint Get(SaveBatchInput<T> parameter)
	{
		var (destination, entities) = parameter;
		var configuration = new BulkConfig
		{
			BatchSize           = entities.Length,
			SqlBulkCopyOptions  = SqlBulkCopyOptions.KeepIdentity,
			PreserveInsertOrder = true, UseTempDB = false,
		};
		destination.BulkInsert(entities, configuration);
		var result = (uint)entities.Length;
		return result;
	}
}