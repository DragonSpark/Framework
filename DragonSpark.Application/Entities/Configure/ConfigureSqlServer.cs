using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace DragonSpark.Application.Entities.Configure;

public sealed class ConfigureSqlServer<T> : ConfigureSqlServer
{
	public static ConfigureSqlServer<T> Default { get; } = new ConfigureSqlServer<T>();

	ConfigureSqlServer()
		: this(new SqlServerMigrations(A.Type<T>().Assembly.GetName().Name.Verify()).Execute) {}

	public ConfigureSqlServer(string name) : this(new SqlServerMigrations(name).Execute) {}

	public ConfigureSqlServer(Action<SqlServerDbContextOptionsBuilder> configure)
		: this(ConnectionString<T>.Default.Get(EnvironmentalConfiguration.Default.Get()), configure) {}

	public ConfigureSqlServer(string connection, Action<SqlServerDbContextOptionsBuilder> name)
		: base(connection, name) {}
}

public class ConfigureSqlServer : ICommand<DbContextOptionsBuilder>
{
	readonly string                                   _connection;
	readonly Action<SqlServerDbContextOptionsBuilder> _configuration;

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