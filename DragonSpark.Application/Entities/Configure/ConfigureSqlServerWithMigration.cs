using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Entities.Configure
{
	public sealed class ConfigureSqlServerWithMigration<T> : Command<DbContextOptionsBuilder<T>> where T : DbContext
	{
		public ConfigureSqlServerWithMigration(Type type) : this(type.Assembly.GetName().Name.Verify()) {}

		public ConfigureSqlServerWithMigration(string name) : base(new ConfigureSqlServer<T>(name)) {}
	}
}