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
	sealed class ConfigureEntityStorage<T> : ICommand<DbContextOptionsBuilder>
	{
		readonly string _connection;
		readonly string _name;

		public ConfigureEntityStorage(string connection) : this(connection, A.Type<T>().Assembly.GetName().Name) {}

		public ConfigureEntityStorage(string connection, string name)
		{
			_connection = connection;
			_name = name;
		}

		public void Execute(DbContextOptionsBuilder parameter)
		{
			parameter.UseSqlServer(_connection, x => x.MigrationsAssembly(_name));
		}
	}

	sealed class ConfigureEntityStorage<T, TUser> : ICommand<IServiceCollection> where T : DbContext where TUser : class
	{
		readonly Action<IdentityOptions> _identity;
		readonly string                  _name;

		public ConfigureEntityStorage(Action<IdentityOptions> identity)
			: this(identity, $"{A.Type<T>().Name}Connection") {}

		public ConfigureEntityStorage(Action<IdentityOptions> identity, string name)
		{
			_identity = identity;
			_name     = name;
		}

		public void Execute(IServiceCollection parameter)
		{
			parameter.AddDbContext<T>(new ConfigureEntityStorage<T>(parameter.Configuration()
			                                                                 .GetConnectionString(_name)).Execute)
			         .AddDefaultIdentity<TUser>(_identity)
			         .AddEntityFrameworkStores<T>();
		}
	}
}