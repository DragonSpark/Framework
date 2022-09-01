using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Entities.Configure;

public sealed class ConfigureSqlServerWithMigration<T> : IStorageConfiguration
	where T : DbContext
{
	readonly IFormatter<IConfiguration>               _connection;
	readonly Action<SqlServerDbContextOptionsBuilder> _configure;

	public ConfigureSqlServerWithMigration(Type type) : this(type.Assembly.GetName().Name.Verify()) {}

	public ConfigureSqlServerWithMigration(string name)
		: this(ConnectionString<T>.Default, new SqlServerMigrations(name).Execute) {}

	public ConfigureSqlServerWithMigration(IFormatter<IConfiguration> connection,
	                                       Action<SqlServerDbContextOptionsBuilder> configure)
	{
		_connection = connection;
		_configure  = configure;
	}

	public Action<DbContextOptionsBuilder> Get(IServiceCollection parameter)
		=> new ConfigureSqlServer(_connection.Get(parameter.Configuration()), _configure).Execute;
}