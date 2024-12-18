using DragonSpark.Application.AspNet.Navigation;
using DragonSpark.Application.AspNet.Navigation.Security;
using DragonSpark.Application.AspNet.Runtime.Operations;
using DragonSpark.Application.AspNet.Security;
using DragonSpark.Application.AspNet.Security.Identity;
using DragonSpark.Application.AspNet.Security.Identity.Model;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.AspNet;

sealed class DefaultRegistrations : ICommand<IServiceCollection>
{
	public static DefaultRegistrations Default { get; } = new();

	DefaultRegistrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IScopedToken>()
				 .Forward<ScopedToken>()
				 .Decorate<AmbientAwareToken>()
				 .Scoped()
				 //
				 .Then.Start<CurrentRootPath>()
				 .And<RedirectLoginPath>()
				 .And<RefreshCurrentPath>()
				 .And<SignOutCurrentPath>()
				 .And<CurrentPath>()
				 .Scoped()
				 //
				 .Then.Start<ICurrentContext>()
				 .Forward<CurrentContext>()
				 .Scoped()
				 //
				 .Then.Start<INavigateToSignOut>()
				 .Forward<NavigateToSignOut>()
				 .Scoped()
				 //
				 .Then.Start<Base64UrlEncrypt>()
				 .And<Base64UrlDecrypt>()
				 .Singleton()
				 //
				 .Then.Start<ICurrentUserNumber>()
				 .Forward<CurrentUserNumber>()
				 .Scoped();
	}
}