using DragonSpark.Application.Entities.Configure;

namespace DragonSpark.Application.Environment;

public sealed class StorageConfiguration : Entities.Configure.StorageConfiguration
{
	public static StorageConfiguration Default { get; } = new StorageConfiguration();

	StorageConfiguration() : base(EmptyStorageConfiguration.Default.Get) {}
}