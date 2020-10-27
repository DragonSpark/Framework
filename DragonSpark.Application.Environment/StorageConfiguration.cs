using DragonSpark.Application.Compose.Entities;

namespace DragonSpark.Application.Environment
{
	public sealed class StorageConfiguration : Compose.Entities.StorageConfiguration
	{
		public static StorageConfiguration Default { get; } = new StorageConfiguration();

		StorageConfiguration() : base(EmptyStorageConfiguration.Default.Get) {}
	}
}