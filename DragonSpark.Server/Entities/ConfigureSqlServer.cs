using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Server.Entities
{
	sealed class ConfigureSqlServer<T> : ICommand<DbContextOptionsBuilder>
	{
		public static ConfigureSqlServer<T> Default { get; } = new ConfigureSqlServer<T>();

		ConfigureSqlServer() : this(ConnectionString<T>.Default.Get(EnvironmentalConfiguration.Default.Get())) {}

		readonly string _connection;
		readonly string _name;

		public ConfigureSqlServer(string connection) : this(connection, A.Type<T>().Assembly.GetName().Name) {}

		public ConfigureSqlServer(string connection, string name)
		{
			_connection = connection;
			_name       = name;
		}

		public void Execute(DbContextOptionsBuilder parameter)
		{
			parameter.UseSqlServer(_connection, x => x.MigrationsAssembly(_name));
		}
	}

	sealed class ConfigureSqlServer<T, TUser> : ICommand<IServiceCollection> where T : DbContext where TUser : class
	{
		readonly Action<IdentityOptions> _identity;
		readonly string                  _name;

		public ConfigureSqlServer(Action<IdentityOptions> identity) : this(identity, ConnectionName<T>.Default) {}

		public ConfigureSqlServer(Action<IdentityOptions> identity, string name)
		{
			_identity = identity;
			_name     = name;
		}

		public void Execute(IServiceCollection parameter)
		{
			parameter.AddDbContext<T>(new ConfigureSqlServer<T>(parameter.Configuration()
			                                                             .GetConnectionString(_name)).Execute)
			         .AddDefaultIdentity<TUser>(_identity)
			         .AddEntityFrameworkStores<T>();
		}
	}
}