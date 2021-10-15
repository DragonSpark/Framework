using DragonSpark.Application.Entities.Configure;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Compose.Entities;

sealed class AddIdentity<T, TContext> : ICommand<IServiceCollection> where TContext : DbContext where T : class
{
	readonly AddFactories<TContext>  _services;
	readonly Action<IdentityOptions> _identity;

	public AddIdentity(IStorageConfiguration storage, ServiceLifetime lifetime = ServiceLifetime.Scoped)
		: this(storage, DefaultContextFactory<TContext>.Default.Get, lifetime) {}

	public AddIdentity(IStorageConfiguration storage, Func<IServiceProvider, TContext> factory,
	                   ServiceLifetime lifetime = ServiceLifetime.Scoped)
		: this(new AddFactories<TContext>(storage, factory, lifetime), _ => {}) {}

	public AddIdentity(IStorageConfiguration storage, Action<IdentityOptions> configure,
	                   ServiceLifetime lifetime = ServiceLifetime.Scoped)
		: this(new AddFactories<TContext>(storage, lifetime), configure) {}

	public AddIdentity(AddFactories<TContext> services, Action<IdentityOptions> identity)
	{
		_services = services;
		_identity = identity;
	}

	public void Execute(IServiceCollection parameter)
	{
		_services.Get(parameter)
		         //
		         .AddDefaultIdentity<T>(_identity)
		         .AddEntityFrameworkStores<TContext>();
	}
}