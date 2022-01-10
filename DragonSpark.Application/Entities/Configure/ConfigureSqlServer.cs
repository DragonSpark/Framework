using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace DragonSpark.Application.Entities.Configure;

public sealed class ConfigureSqlServer<T> : ConfigureSqlServer
{
	public ConfigureSqlServer(Type migrations)
		: this(ConnectionString<T>.Default.Get(EnvironmentalConfiguration.Default.Get()), migrations) {}

	public ConfigureSqlServer(string connection, Type migrations)
		: this(connection, new SqlServerMigrations(migrations).Execute) {}

	public ConfigureSqlServer(string connection, string name)
		: this(connection, new SqlServerMigrations(name).Execute) {}

	public ConfigureSqlServer(string connection, Action<SqlServerDbContextOptionsBuilder> name)
		: base(connection, name) {}
}

public class ConfigureSqlServer : ICommand<DbContextOptionsBuilder>
{
	readonly string                                   _connection;
	readonly Action<SqlServerDbContextOptionsBuilder> _configuration;

	public ConfigureSqlServer(string connection) : this(connection, _ => {}) {}

	protected ConfigureSqlServer(string connection, Action<SqlServerDbContextOptionsBuilder> configuration)
	{
		_connection    = connection;
		_configuration = configuration;
	}

	public void Execute(DbContextOptionsBuilder parameter)
	{
		parameter.UseSqlServer(_connection, _configuration);
	}
}