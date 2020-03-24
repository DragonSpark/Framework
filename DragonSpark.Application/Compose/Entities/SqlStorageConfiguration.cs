using DragonSpark.Composition;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose.Entities
{
	sealed class SqlStorageConfiguration<T> : IStorageConfiguration where T : DbContext
	{
		public static SqlStorageConfiguration<T> Default { get; } = new SqlStorageConfiguration<T>();

		SqlStorageConfiguration() : this(ConnectionName<T>.Default) {}

		readonly string _name;

		public SqlStorageConfiguration(string name) => _name = name;

		public Action<DbContextOptionsBuilder> Get(IServiceCollection parameter)
			=> new ConfigureSqlServer<T>(parameter.Configuration().GetConnectionString(_name)).Execute;
	}
}