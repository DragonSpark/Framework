using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Application.Security.Identity.Claims.Compile;
using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity;

public sealed class CommonRegistrations<TContext, T> : ICommand<IServiceCollection>
	where TContext : DbContext
	where T : IdentityUser

{
	public static CommonRegistrations<TContext, T> Default { get; } = new();

	CommonRegistrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.AddSingleton<IKnownClaims>(KnownClaims.Default)
		         .AddSingleton(typeof(IAuthentications<>), typeof(Authentications<>))
		         .AddSingleton(typeof(IUsers<>), typeof(Users<>))
		         .AddSingleton(typeof(UserSessions<>));
	}
}