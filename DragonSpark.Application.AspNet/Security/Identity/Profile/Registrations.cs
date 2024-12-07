using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class Registrations<T> : ICommand<IServiceCollection> where T : IdentityUser
{
	public static Registrations<T> Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IUserSynchronization>()
		         .Forward<UserSynchronization<T>>()
		         .Scoped()
		         //
		         .Then.Start<IUserSynchronizer<T>>()
		         .Forward<UserSynchronizer<T>>()
		         .Singleton()
		         //
		         .Then.Start<ICreateRequest>()
		         .Forward<CreateRequest<T>>()
		         .Include(x => x.Dependencies.Recursive())
		         .Scoped()
		         //
		         .Then.Start<INew<T>>()
		         .Forward<New<T>>()
		         .Singleton()
		         //
		         .Then.Start<ICreateExternal<T>>()
		         .Forward<CreateExternal<T>>()
		         .Decorate<SynchronizationAwareCreateExternal<T>>()
		         .Decorate<LoggingAwareCreateExternal<T>>()
		         .Decorate<FailureAwareCreateExternal<T>>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         //
		         .Then.Start<ICreate<T>>()
		         .Forward<Create<T>>()
		         .Decorate<AddLoginAwareCreate<T>>()
		         .Singleton()
		         //
		         .Then.Start<ILocateUser<T>>()
		         .Forward<LocateUser<T>>()
		         .Singleton()
			;
	}
}