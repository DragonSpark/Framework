using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Entities.Configure;

sealed class ConfigureSqlServerWithMigration<T> : ConfigureSqlServerWithMigration where T : DbContext
{
	public ConfigureSqlServerWithMigration(Type type) : this(type.Assembly.GetName().Name.Verify()) {}

	public ConfigureSqlServerWithMigration(string name)
		: this(ConnectionString<T>.Default, new SqlServerMigrations(name).Execute) {}

	public ConfigureSqlServerWithMigration(IFormatter<IConfiguration> connection,
	                                       Action<SqlServerDbContextOptionsBuilder> configure)
		: base(connection, configure) {}
}

class ConfigureSqlServerWithMigration : IStorageConfiguration
{
	readonly IFormatter<IConfiguration>               _connection;
	readonly Action<SqlServerDbContextOptionsBuilder> _configure;

	public ConfigureSqlServerWithMigration(string name, Type type)
		: this(name, type.Assembly.GetName().Name.Verify()) {}

	public ConfigureSqlServerWithMigration(string name, string migrations)
		: this(new ConnectionString(name), new SqlServerMigrations(migrations).Execute) {}

	public ConfigureSqlServerWithMigration(IFormatter<IConfiguration> connection,
	                                       Action<SqlServerDbContextOptionsBuilder> configure)
	{
		_connection = connection;
		_configure  = configure;
	}

	public Action<DbContextOptionsBuilder> Get(IServiceCollection parameter)
		=> new UseSqlServer(_connection.Get(parameter.Configuration()), _configure).Execute;
}