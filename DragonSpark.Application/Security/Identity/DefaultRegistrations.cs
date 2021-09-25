using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity
{
	sealed class DefaultRegistrations<TContext, T> : ICommand<IServiceCollection>
		where T : IdentityUser where TContext : DbContext
	{
		public static DefaultRegistrations<TContext, T> Default { get; } = new();

		DefaultRegistrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<IMarkModified<T>>()
			         .Forward<MarkModified<TContext, T>>()
			         .Scoped()
			         //
			         .Then.Start<IHasValidPrincipalState>()
			         .Forward<HasValidPrincipalState<T>>()
			         .Singleton()
			         //
			         .Then.Start<IsCurrentSecurityStateValid>()
			         .Singleton()
			         //
			         .Then.Start<IHasValidState<T>>()
			         .Forward<HasValidState<T>>()
			         .Singleton()
			         //
			         .Then.Start<CurrentProviderIdentity>()
			         .Singleton()
			         //
			         .Then.AddSingleton(typeof(IUsers<>), typeof(Users<>))
			         
				;
		}
	}
}