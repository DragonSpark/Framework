using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using EFCore.BulkExtensions;
using System.Linq;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators.Save;

sealed class Insert<T> : ISave<T> where T : class
{
	public static Insert<T> Default { get; } = new();

	Insert() {}

	public async ValueTask<uint> Get(Stop<SaveInput<T>> parameter)
	{
		var ((logger, size, destination, _, total), stop) = parameter;
		var configuration = new BulkConfig
		{
			BatchSize           = size,
			SqlBulkCopyOptions  = SqlBulkCopyOptions.KeepIdentity,
			PreserveInsertOrder = true, UseTempDB              = false,
			NotifyAfter         = size, EnableShadowProperties = true
		};

		var       progress = new Progress<T>(logger, total).Execute;
		foreach (var changed in destination.ChangeTracker.Entries()
		                                   .Where(x => !x.Metadata.IsOwned())
		                                   .GroupBy(x => x.Entity.GetType()))
		{
			configuration.PropertiesToExclude?.Clear();
			await destination.BulkInsertAsync(changed.Select(x => x.Entity).ToList(), configuration, progress,
			                                  cancellationToken: stop)
			                 .Off();
		}

		return total;
	}
}