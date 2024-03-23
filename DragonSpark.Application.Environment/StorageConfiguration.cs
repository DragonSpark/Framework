using DragonSpark.Application.Entities.Configure;
using JetBrains.Annotations;

namespace DragonSpark.Application.Environment;

public sealed class StorageConfiguration : Entities.Configure.StorageConfiguration
{
	[UsedImplicitly]
	public static StorageConfiguration Default { get; } = new();

	StorageConfiguration() : base(EmptyStorageConfiguration.Default.Get) {}
}