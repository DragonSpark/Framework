using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace DragonSpark.Application.Entities.Configure;

public sealed class ConfigureSqlServer<T> : ConfigureSqlServer
{
	public ConfigureSqlServer(string name) : this(new SqlServerMigrations(name).Execute) {}

	public ConfigureSqlServer(Type migrations) : this(new SqlServerMigrations(migrations).Execute) {}

	public ConfigureSqlServer(Action<SqlServerDbContextOptionsBuilder> configure)
		: base(ConnectionString<T>.Default.Get(EnvironmentalConfiguration.Default.Get()), configure) {}
}

public class ConfigureSqlServer : ICommand<DbContextOptionsBuilder>
{
	readonly string                                   _connection;
	readonly Action<SqlServerDbContextOptionsBuilder> _configuration;

	public ConfigureSqlServer(string connection) : this(connection, _ => {}) {}

	public ConfigureSqlServer(string connection, Action<SqlServerDbContextOptionsBuilder> configuration)
	{
		_connection    = connection;
		_configuration = configuration;
	}

	public void Execute(DbContextOptionsBuilder parameter)
	{
		parameter.UseSqlServer(_connection, _configuration);
	}
}