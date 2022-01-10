using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Entities.Configure;

public sealed class ConfigureSqlServerWithMigration<T> : IStorageConfiguration
	where T : DbContext
{
	readonly IFormatter<IConfiguration> _connection;
	readonly string                     _name;

	public ConfigureSqlServerWithMigration(Type type) : this(type.Assembly.GetName().Name.Verify()) {}

	public ConfigureSqlServerWithMigration(string name) : this(ConnectionString<T>.Default, name) {}

	public ConfigureSqlServerWithMigration(IFormatter<IConfiguration> connection, string name)
	{
		_connection = connection;
		_name       = name;
	}

	public Action<DbContextOptionsBuilder> Get(IServiceCollection parameter)
		=> new ConfigureSqlServer<T>(_connection.Get(parameter.Configuration()), _name).Execute;
}