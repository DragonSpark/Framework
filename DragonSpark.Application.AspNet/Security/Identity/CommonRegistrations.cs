using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Application.AspNet.Security.Identity.Claims.Compile;
using DragonSpark.Application.AspNet.Security.Identity.Model;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet.Security.Identity;

sealed class CommonRegistrations<TContext, T> : ICommand<IServiceCollection>
    where TContext : DbContext
    where T : IdentityUser

{
    public static CommonRegistrations<TContext, T> Default { get; } = new();

    CommonRegistrations() : this(Entities.Configuration.Registrations.Default) {}

    readonly ICommand<IServiceCollection> _previous;

    public CommonRegistrations(ICommand<IServiceCollection> previous) => _previous = previous;

    public void Execute(IServiceCollection parameter)
    {
        _previous.Execute(parameter);
        parameter.Start<CurrentContextUser<object>>()
                 .Generic()
                 .Include(x => x.Dependencies.Recursive())
                 .Singleton()
                 //
                 .Then.AddSingleton<IKnownClaims>(KnownClaims.Default)
                 .AddSingleton(typeof(IAuthentications<>), typeof(Authentications<>))
                 .AddSingleton(typeof(IUsers<>), typeof(Users<>))
                 .AddSingleton(typeof(ICurrent<>), typeof(Current<>))
                 .AddSingleton(typeof(UserSessions<>));
    }
}