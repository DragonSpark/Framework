using DragonSpark.Application.AspNet.Entities.Initialization;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;

namespace DragonSpark.Application.AspNet.Entities.Configure;

public sealed class UseSqlServer<T> : UseSqlServer
{
	public UseSqlServer(string name) : this(new SqlServerMigrations(name).Execute) {}

	public UseSqlServer(Type migrations) : this(new SqlServerMigrations(migrations).Execute) {}

	public UseSqlServer(Action<SqlServerDbContextOptionsBuilder> configure)
		: base(ConnectionString<T>.Default.Get(EnvironmentalConfiguration.Default.Get()), configure) {}
}

public class UseSqlServer : ICommand<DbContextOptionsBuilder>
{
	readonly string                                   _connection;
	readonly Action<SqlServerDbContextOptionsBuilder> _configuration;

	public UseSqlServer(string connection) : this(connection, _ => {}) {}

	public UseSqlServer(string connection, Action<SqlServerDbContextOptionsBuilder> configuration)
	{
		_connection    = connection;
		_configuration = configuration;
	}

	public void Execute(DbContextOptionsBuilder parameter)
	{
		parameter.UseSqlServer(_connection, _configuration).UseAsyncSeeding(ApplyMigrationRegistry.Default.Get); // TODO: move to centralized area
	}
}