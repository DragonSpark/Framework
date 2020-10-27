using JetBrains.Annotations;

namespace DragonSpark.Application.Environment
{
	public sealed class StorageConfiguration : DragonSpark.Application.Compose.Entities.StorageConfiguration
	{
		[UsedImplicitly]
		public static StorageConfiguration Default { get; } = new StorageConfiguration();

		StorageConfiguration() : base(x => x.EnableSensitiveDataLogging()) {}
	}
}