using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Application.Compose.Entities
{
	sealed class ConfigureSqlServer<T> : ICommand<DbContextOptionsBuilder>
	{
		public static ConfigureSqlServer<T> Default { get; } = new ConfigureSqlServer<T>();

		ConfigureSqlServer() : this(ConnectionString<T>.Default.Get(EnvironmentalConfiguration.Default.Get())) {}

		readonly string _connection;
		readonly string _name;

		public ConfigureSqlServer(string connection) : this(connection, A.Type<T>().Assembly.GetName().Name) {}

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