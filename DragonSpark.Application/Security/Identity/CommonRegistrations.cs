using DragonSpark.Application.Security.Identity.Authentication;
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
		parameter.AddSingleton(typeof(IAuthentications<>), typeof(Authentications<>))
		         .AddSingleton(typeof(IUsers<>), typeof(Users<>));
	}
}