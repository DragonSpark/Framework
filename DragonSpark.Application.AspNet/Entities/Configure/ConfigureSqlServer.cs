using DragonSpark.Composition;
using DragonSpark.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.AspNet.Entities.Configure;

sealed class ConfigureSqlServer<T> : IStorageConfiguration where T : DbContext
{
	readonly IFormatter<IConfiguration>               _connection;
	readonly Action<SqlServerDbContextOptionsBuilder> _configure;

	public ConfigureSqlServer(Action<SqlServerDbContextOptionsBuilder> configure)
		: this(ConnectionString<T>.Default, configure) {}

	public ConfigureSqlServer(IFormatter<IConfiguration> connection, Action<SqlServerDbContextOptionsBuilder> configure)
	{
		_connection = connection;
		_configure  = configure;
	}

	public Action<DbContextOptionsBuilder> Get(IServiceCollection parameter)
		=> new UseSqlServer(_connection.Get(parameter.Configuration()), _configure).Execute;
}

class ConfigureSqlServer : IStorageConfiguration
{
	readonly IFormatter<IConfiguration>               _connection;
	readonly Action<SqlServerDbContextOptionsBuilder> _configure;

	public ConfigureSqlServer(string name, Action<SqlServerDbContextOptionsBuilder> configure)
		: this(new ConnectionString(name), configure) {}

	public ConfigureSqlServer(IFormatter<IConfiguration> connection, Action<SqlServerDbContextOptionsBuilder> configure)
	{
		_connection = connection;
		_configure  = configure;
	}

	public Action<DbContextOptionsBuilder> Get(IServiceCollection parameter)
		=> new UseSqlServer(_connection.Get(parameter.Configuration()), _configure).Execute;
}