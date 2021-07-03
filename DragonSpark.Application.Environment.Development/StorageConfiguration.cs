using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DragonSpark.Application.Environment
{
	public sealed class StorageConfiguration : Compose.Entities.StorageConfiguration
	{
		[UsedImplicitly]
		public static StorageConfiguration Default { get; } = new StorageConfiguration();

		StorageConfiguration()
			: base(x => x.EnableSensitiveDataLogging()
			             .EnableDetailedErrors()
			             .ConfigureWarnings(y => y.Throw(RelationalEventId.MultipleCollectionIncludeWarning)
			                                      .Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS))) {}
	}
}