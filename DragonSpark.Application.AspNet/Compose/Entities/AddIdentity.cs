using DragonSpark.Application.AspNet.Entities.Configure;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Compose.Entities;

sealed class AddIdentity<T, TContext> : ICommand<IServiceCollection> where TContext : DbContext where T : class
{
    readonly AddFactories<TContext>  _services;
    readonly Action<IdentityOptions> _identity;
    readonly Action<IdentityBuilder> _builder;

    // ReSharper disable once TooManyDependencies
    public AddIdentity(IStorageConfiguration storage, Action<IdentityOptions> configure,
                       Action<IdentityBuilder> builder, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        : this(new AddFactories<TContext>(storage, lifetime), configure, builder) {}

    public AddIdentity(AddFactories<TContext> services, Action<IdentityOptions> identity,
                       Action<IdentityBuilder> builder)
    {
        _services     = services;
        _identity     = identity;
        _builder = builder;
    }

    public void Execute(IServiceCollection parameter)
    {
        _services.Get(parameter)
                 .AddAuthentication(options =>
                                    {
                                        options.DefaultScheme       = IdentityConstants.ApplicationScheme;
                                        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                                    })
                 .AddIdentityCookies();
        //
        var builder = parameter.AddIdentityCore<T>(_identity)
                               .AddEntityFrameworkStores<TContext>()
                               .AddSignInManager()
                               .AddDefaultTokenProviders();
        _builder(builder);
    }
}