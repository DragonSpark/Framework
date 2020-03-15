using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application.Security.Identity
{
	sealed class IdentityRegistration<T> : ICommand<IServiceCollection> where T : IdentityUser
	{
		readonly IAppliedPrincipal          _principal;
		readonly IClaims                    _claims;
		readonly Func<ExternalLoginInfo, T> _new;

		public IdentityRegistration(IAppliedPrincipal principal, IClaims claims, Func<ExternalLoginInfo, T> @new)
		{
			_principal = principal;
			_claims    = claims;
			_new       = @new;
		}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Scan(x => x.FromAssemblyOf<IdentityRegistration<T>>()
			                     .AddClasses(y => y.InExactNamespaces(typeof(IdentityRegistration<T>).Namespace,
			                                                          typeof(ExternalLoginModel<T>).Namespace))
			                     .AsSelf()
			                     .AsMatchingInterface()
			                     .WithScopedLifetime())
			         .AddSingleton(_claims)
			         .AddSingleton(_new)
			         .AddScoped<IUserSynchronization, UserSynchronization<T>>()
			         .AddScoped<ICreateAction, CreateAction<T>>()
			         .Decorate<ICreateAction, SynchronizedCreateAction>()
			         .AddScoped<IExternalSignin, ExternalSignin<T>>()
			         // Profile:
			         .AddSingleton(_principal)
			         .AddScoped<IAuthenticationProfile, AuthenticationProfile<T>>()
			         .Decorate<IAuthenticationProfile, AuthenticationProfile>()
			         //
			         .AddControllers(x => x.ModelBinderProviders.Insert(0, ModelBinderProvider.Default));
		}
	}
}