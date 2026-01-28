using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using EFCore.BulkExtensions;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class Insert<T> : ISave<T> where T : class
{
	public static Insert<T> Default { get; } = new();

	Insert() {}

	public async ValueTask<uint> Get(Stop<SaveInput<T>> parameter)
	{
		var ((logger, size, destination, entities, total), stop) = parameter;
		var configuration = new BulkConfig
		{
			BatchSize           = size,
			SqlBulkCopyOptions  = SqlBulkCopyOptions.KeepIdentity,
			PreserveInsertOrder = true, UseTempDB = false,
			NotifyAfter         = size,
		};

		await destination.BulkInsertAsync(entities, configuration, new Progress<T>(logger, total).Execute, 
		                                  cancellationToken: stop).Off();
		return total;
	}
}