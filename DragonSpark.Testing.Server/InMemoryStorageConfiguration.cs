using DragonSpark.Application.Compose.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Testing.Server
{
	public sealed class InMemoryStorageConfiguration : IStorageConfiguration
	{
		public static InMemoryStorageConfiguration Default { get; } = new InMemoryStorageConfiguration();

		InMemoryStorageConfiguration() {}

		public Action<DbContextOptionsBuilder> Get(IServiceCollection parameter)
			=> InMemoryConfiguration.Default.Execute;
	}
}