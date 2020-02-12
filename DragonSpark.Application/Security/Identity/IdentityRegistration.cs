using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Security.Identity
{
	public class IdentityRegistration<T> : ICommand<IServiceCollection> where T : IdentityUser
	{
		readonly IClaims                    _claims;
		readonly Func<ExternalLoginInfo, T> _new;

		public IdentityRegistration(IClaims claims, Func<ExternalLoginInfo, T> @new)
		{
			_claims = claims;
			_new    = @new;
		}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Scan(y => y.FromAssemblyOf<IdentityRegistration<T>>()
			                     .AddClasses(z => z.InExactNamespaces(typeof(IdentityRegistration<T>).Namespace,
			                                                          typeof(ExternalLoginModel<T>).Namespace))
			                     .AsSelf()
			                     .AsMatchingInterface()
			                     .WithScopedLifetime())
			         .AddSingleton(_claims)
			         .AddSingleton(_new)
			         .AddScoped<IUserSynchronization, UserSynchronization<T>>()
			         .AddScoped<ICreateAction, CreateAction<T>>()
			         .AddScoped<IExternalSignin, ExternalSignin<T>>()
			         .AddScoped<IAuthenticateAction, AuthenticateAction<T>>();
		}
	}
}