using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DragonSpark.Application.Environment;

public sealed class StorageConfiguration : AspNet.Entities.Configure.StorageConfiguration
{
	[UsedImplicitly]
	public static StorageConfiguration Default { get; } = new();

	StorageConfiguration()
		: base(x => x.ConfigureWarnings(y => y.Ignore(CoreEventId.DistinctAfterOrderByWithoutRowLimitingOperatorWarning))) {}
}