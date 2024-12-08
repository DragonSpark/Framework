using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DragonSpark.Application.Environment.Development;

public sealed class StorageConfiguration : AspNet.Entities.Configure.StorageConfiguration
{
	[UsedImplicitly]
	public static StorageConfiguration Default { get; } = new();

	StorageConfiguration()
		: base(x => x.AddInterceptors(AzureAdAuthenticationDbConnectionInterceptor.Default)
		             .EnableSensitiveDataLogging()
		             .EnableDetailedErrors()
		             .ConfigureWarnings(y => y.Throw(RelationalEventId.MultipleCollectionIncludeWarning))) {}
}