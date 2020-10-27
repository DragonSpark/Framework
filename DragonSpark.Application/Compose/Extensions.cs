using DragonSpark.Application.Compose.Entities;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Compose
{
	public static class Extensions
	{
		public static StorageConfigurationBuilder WithSqlServer<T>(this StorageConfigurationBuilder @this)
			where T : DbContext
			=> @this.Append(ConfigureSqlServer<T>.Default.Execute);

		public static StorageConfigurationBuilder WithEnvironmentalConfiguration(this StorageConfigurationBuilder @this)
			=> @this.Append(EnvironmentalStorageConfiguration.Default);
	}
}
