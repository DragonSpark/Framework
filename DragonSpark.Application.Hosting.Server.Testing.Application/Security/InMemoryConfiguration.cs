using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Hosting.Server.Testing.Application.Security
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