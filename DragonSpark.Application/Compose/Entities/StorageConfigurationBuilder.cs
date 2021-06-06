using DragonSpark.Model.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class StorageConfigurationBuilder : IResult<IStorageConfiguration>
	{
		readonly IStorageConfiguration _configuration;

		public StorageConfigurationBuilder() : this(EmptyStorageConfiguration.Default) {}

		public StorageConfigurationBuilder(IStorageConfiguration configuration) => _configuration = configuration;

		public StorageConfigurationBuilder Append(Func<IServiceCollection, Action<DbContextOptionsBuilder>> configure)
			=> Append(new StorageConfiguration(configure));

		public StorageConfigurationBuilder Append(Action<DbContextOptionsBuilder> configure)
			=> Append(new StorageConfiguration(configure));

		public StorageConfigurationBuilder Append(IStorageConfiguration append)
			=> new (new AppendedStorageConfiguration(_configuration, append));

		public IStorageConfiguration Get() => _configuration;
	}
}