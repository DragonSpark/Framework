using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.Model
{
	public sealed class Registrations<T> : ICommand<IServiceCollection> where T : class
	{
		public static Registrations<T> Default { get; } = new Registrations<T>();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<IAuthenticationRequest>()
			         .Forward<AuthenticationRequest>()
			         .Scoped()
			         //
			         .Then.Start<ExternalLoginChallengingModelBinder>()
			         .And<ChallengedModelBinder>()
			         .And<ReturnOrRoot>()
			         .And<ExternalLoginModel<T>>()
			         .Include(x => x.Dependencies)
			         .Scoped();
		}
	}
}