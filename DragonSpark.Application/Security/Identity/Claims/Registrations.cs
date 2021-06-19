using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.Claims
{
	public sealed class Registrations : ICommand<IServiceCollection>
	{
		public static Registrations Default { get; } = new Registrations();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<IClaims>()
			         .Forward<Claims>()
			         .Scoped()
			         .Then.Start<ICurrentKnownClaims>()
			         .Forward<CurrentKnownClaims>()
			         .Scoped()
			         .Then.Start<IDisplayNameClaim>()
			         .Forward<DisplayNameClaim>()
			         .Scoped()
			         //
			         .Then.Start<IExtractClaims>()
			         .Forward<ExtractClaims>()
			         .Singleton()
			         .Then.AddSingleton<IKnownClaims>(KnownClaims.Default);
		}
	}
}