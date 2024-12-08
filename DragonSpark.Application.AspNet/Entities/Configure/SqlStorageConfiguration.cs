using DragonSpark.Compose;
using DragonSpark.Composition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.AspNet.Entities.Configure;

sealed class SqlStorageConfiguration<T> : IStorageConfiguration where T : DbContext
{
	public static SqlStorageConfiguration<T> Default { get; } = new();

	SqlStorageConfiguration() : this(_ => {}) {}

	readonly string                                   _name;
	readonly Action<SqlServerDbContextOptionsBuilder> _configuration;

	public SqlStorageConfiguration(Action<SqlServerDbContextOptionsBuilder> configuration)
		: this(ConnectionName<T>.Default, configuration) {}

	public SqlStorageConfiguration(string name, Action<SqlServerDbContextOptionsBuilder> configuration)
	{
		_name          = name;
		_configuration = configuration;
	}

	public Action<DbContextOptionsBuilder> Get(IServiceCollection parameter)
		=> new UseSqlServer(parameter.Configuration().GetConnectionString(_name).Verify(), _configuration).Execute;
}