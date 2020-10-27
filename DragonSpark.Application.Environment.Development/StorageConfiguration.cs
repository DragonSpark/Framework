using DragonSpark.Application.Compose.Entities;
using JetBrains.Annotations;

namespace DragonSpark.Application.Environment
{
	public sealed class StorageConfiguration : Compose.Entities.StorageConfiguration
	{
		[UsedImplicitly]
		public static StorageConfiguration Default { get; } = new StorageConfiguration();

		StorageConfiguration() : base(EmptyStorageConfiguration.Default.Get) {}
	}
}