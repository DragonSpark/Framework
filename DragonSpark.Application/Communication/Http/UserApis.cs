using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Stores;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;

namespace DragonSpark.Application.Communication.Http;

public sealed class UserApis<T> : ReferenceValueStore<ClaimsPrincipal, T> where T : class
{
	public UserApis(IBearer bearer, Func<T> client)
		: base(new AuthenticatedApi<T>(new ApplicationUserAwareBearer(bearer), client)) {}
}

// TODO
sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<UserApis<object>>()
		         .Generic()
		         .Singleton();
	}
}