using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Testing.Server
{
	sealed class InMemoryConfiguration : ICommand<DbContextOptionsBuilder>
	{
		public static InMemoryConfiguration Default { get; } = new InMemoryConfiguration();

		InMemoryConfiguration() {}

		public void Execute(DbContextOptionsBuilder parameter)
		{
			parameter.UseInMemoryDatabase(Guid.NewGuid().ToString())
			         .EnableSensitiveDataLogging();
		}
	}
}