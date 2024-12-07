using DragonSpark.Application.Navigation;
using DragonSpark.Application.Navigation.Security;
using DragonSpark.Application.Security;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application;

sealed class DefaultRegistrations : ICommand<IServiceCollection>
{
	public static DefaultRegistrations Default { get; } = new();

	DefaultRegistrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<CurrentRootPath>()
		         .And<RedirectLoginPath>()
		         .And<RefreshCurrentPath>()
		         .And<CurrentPath>()
		         .Scoped()
		         //
		         .Then.Start<ICurrentContext>()
		         .Forward<CurrentContext>()
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