using DragonSpark.Application.Security;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Entities
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



	sealed class ConfigureIdentityStorage<T, TUser> : ICommand<IServiceCollection> where T : DbContext where TUser : class
	{
		readonly IStorageConfiguration _storage;
		readonly Action<IdentityOptions> _identity;

		public ConfigureIdentityStorage(IStorageConfiguration storage, Action<IdentityOptions> identity)
		{
			_storage = storage;
			_identity = identity;
		}

		public void Execute(IServiceCollection parameter)
		{
			parameter.AddDbContext<T>(_storage.Get(parameter))
			         .AddDefaultIdentity<TUser>(_identity)
			         .AddEntityFrameworkStores<T>()
			         .Return(parameter)
			         .AddScoped<AuthenticationStateProvider, Revalidation<TUser>>()
			         .AddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipals<TUser>>();
		}
	}
}