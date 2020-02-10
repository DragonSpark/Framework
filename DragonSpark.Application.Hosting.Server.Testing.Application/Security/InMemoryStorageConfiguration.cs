using DragonSpark.Application.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Hosting.Server.Testing.Application.Security
{
	sealed class InMemoryStorageConfiguration : IStorageConfiguration
	{
		public static InMemoryStorageConfiguration Default { get; } = new InMemoryStorageConfiguration();

		InMemoryStorageConfiguration() {}

		public Action<DbContextOptionsBuilder> Get(IServiceCollection parameter)
			=> InMemoryConfiguration.Default.Execute;
	}
}