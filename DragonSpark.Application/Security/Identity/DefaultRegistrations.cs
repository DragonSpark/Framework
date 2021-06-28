using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity
{
	sealed class DefaultRegistrations<T> : ICommand<IServiceCollection> where T : IdentityUser
	{
		public static DefaultRegistrations<T> Default { get; } = new DefaultRegistrations<T>();

		DefaultRegistrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<IMarkModified<T>>()
			         .Forward<MarkModified<T>>()
			         .Scoped()
			         //
			         .Then.Start<IHasValidSecurityState>()
			         .Forward<HasValidSecurityState<T>>()
			         .Scoped()
			         //
			         .Then.Start<IsCurrentSecurityStateValid>()
			         .Scoped();
		}
	}
}