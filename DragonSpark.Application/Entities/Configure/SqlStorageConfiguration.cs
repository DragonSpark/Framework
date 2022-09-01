using DragonSpark.Composition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Entities.Configure;

sealed class SqlStorageConfiguration<T> : IStorageConfiguration where T : DbContext
{
	public static SqlStorageConfiguration<T> Default { get; } = new SqlStorageConfiguration<T>();

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
		=> new ConfigureSqlServer(parameter.Configuration().GetConnectionString(_name), _configuration).Execute;
}