using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	public sealed class ConfigureSqlServer<T> : ConfigureSqlServer
	{
		public static ConfigureSqlServer<T> Default { get; } = new ConfigureSqlServer<T>();

		ConfigureSqlServer() : this(A.Type<T>().Assembly.GetName().Name ?? throw new InvalidOperationException()) {}

		public ConfigureSqlServer(string name)
			: this(ConnectionString<T>.Default.Get(EnvironmentalConfiguration.Default.Get()), name) {}

		public ConfigureSqlServer(string connection, string name) : base(connection, name) {}
	}

	public class ConfigureSqlServer : ICommand<DbContextOptionsBuilder>
	{
		readonly string _connection;
		readonly string _name;

		public ConfigureSqlServer(string connection, string name)
		{
			_connection = connection;
			_name       = name;
		}

		public void Execute(DbContextOptionsBuilder parameter)
		{
			parameter.UseSqlServer(_connection, x => x.MigrationsAssembly(_name));
		}
	}
}