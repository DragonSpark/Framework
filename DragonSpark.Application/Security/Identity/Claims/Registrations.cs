using DragonSpark.Application.Security.Identity.Claims.Compile;
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
			         .Forward<Compile.Claims>()
			         .Include(x => x.Dependencies)
			         .Scoped()
			         .Then.Start<ICurrentKnownClaims>()
			         .Forward<CurrentKnownClaims>()
			         .Scoped()
			         .Then.Start<IDisplayNameClaim>()
			         .Forward<DisplayNameClaim>()
			         .Singleton()
			         //
			         .Then.Start<IExtractClaims>()
			         .Forward<ExtractClaims>()
			         .Singleton()
			         .Then.AddSingleton<IKnownClaims>(KnownClaims.Default);
		}
	}
}