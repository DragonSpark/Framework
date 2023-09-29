using DragonSpark.Application.Diagnostics;
using DragonSpark.Application.Navigation;
using DragonSpark.Application.Runtime.Operations;
using DragonSpark.Application.Security;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application;

sealed class DefaultRegistrations : ICommand<IServiceCollection>
{
	public static DefaultRegistrations Default { get; } = new DefaultRegistrations();

	DefaultRegistrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IToken>()
		         .Forward<Token>()
		         .Decorate<AmbientAwareToken>()
		         .Scoped()
		         //
		         .Then.Start<INavigateToSignOut>()
		         .Forward<NavigateToSignOut>()
		         .Scoped()
		         //
		         .Then.Start<NavigateToSignIn>()
		         .And<CurrentRootPath>()
		         .And<RedirectLoginPath>()
		         .And<SignOutCurrentPath>()
		         .Scoped()
		         //
		         .Then.Start<RefreshCurrentPath>()
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
		         .Then.Start<ILogException>()
		         .Forward<LogException>()
		         .Decorate<TemplateAwareLogException>()
		         .Singleton()
		         //
		         .Then.Start<IExceptionLogger>()
		         .Forward<ExceptionLogger>()
		         .Singleton()
		         //
		         .Then.Start<IExceptions>()
		         .Forward<Exceptions>()
		         .Decorate<ContextAwareExceptions>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<IExecuteOperation>()
		         .Forward<ExecuteOperation>()
		         .Scoped()
		         //
		         .Then.Start<ICurrentUserNumber>()
		         .Forward<CurrentUserNumber>()
		         .Scoped();
	}
}