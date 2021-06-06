using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity
{
	sealed class IdentityRegistration<T> : ICommand<IServiceCollection> where T : IdentityUser
	{
		readonly IClaims _claims;

		public IdentityRegistration(IClaims claims) => _claims = claims;

		public void Execute(IServiceCollection parameter)
		{
			// Performance:
			parameter.Scan(x => x.FromAssemblyOf<IdentityRegistration<T>>()
			                     .AddClasses(y => y.InExactNamespaces(typeof(IdentityRegistration<T>).Namespace!,
			                                                          typeof(ExternalLoginModel<T>).Namespace!))
			                     .AsSelf()
			                     .AsMatchingInterface()
			                     .WithScopedLifetime())
			         .AddSingleton(_claims)
			         .AddScoped<IUserSynchronization, UserSynchronization<T>>()
					 //
			         .AddScoped<ICreateAction, CreateAction<T>>()
			         .Decorate<ICreateAction, SynchronizedCreateAction>()
					 //
			         .AddScoped<IExternalSignin, ExternalSignin<T>>()
			         // Profile:
			         .AddScoped<IAuthenticationProfile, AuthenticationProfile<T>>()
			         .Decorate<IAuthenticationProfile, AuthenticationProfile>()
			         //
			         .AddControllers(x => x.ModelBinderProviders.Insert(0, ModelBinderProvider.Default));
		}
	}
}