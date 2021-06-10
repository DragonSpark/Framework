using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity
{
	sealed class Registrations<T> : ICommand<IServiceCollection> where T : IdentityUser
	{
		public static Registrations<T> Default { get; } = new Registrations<T>();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			// Performance:
			parameter.Scan(x => x.FromAssemblyOf<Registrations<T>>()
			                     .AddClasses(y => y.InExactNamespaces(typeof(Registrations<T>).Namespace!,
			                                                          typeof(ExternalLoginModel<T>).Namespace!))
			                     .AsSelf()
			                     .AsMatchingInterface()
			                     .WithScopedLifetime())
			         //
			         .AddScoped<IUserSynchronization, UserSynchronization<T>>()
			         //
			         .AddScoped<ICreateRequest, CreateRequest<T>>()
			         //
			         .Start<ICreate<T>>()
			         .Forward<Create<T>>()
			         .Decorate<LoggingAwareCreate<T>>()
			         .Scoped()
			         .Then.Start<ICreated<T>>()
			         .Forward<Created<T>>()
			         .Decorate<AddLoginAwareCreated<T>>()
			         .Decorate<SynchronizationAwareCreated<T>>()
			         .Scoped()
			         //
			         .Then.AddScoped<IExternalSignin, ExternalSignin<T>>()
			         // Profile:
			         .AddScoped<IAuthenticationProfile, AuthenticationProfile<T>>()
			         //
			         .AddControllers(x => x.ModelBinderProviders.Insert(0, ModelBinderProvider.Default));
		}
	}
}