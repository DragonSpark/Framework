using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class Registrations<T> : ICommand<IServiceCollection> where T : IdentityUser
{
	public static Registrations<T> Default { get; } = new Registrations<T>();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IUserSynchronization>()
		         .Forward<UserSynchronization<T>>()
		         .Singleton()
		         //
		         .Then.Start<IUserSynchronizer<T>>()
		         .Forward<UserSynchronizer<T>>()
		         .Singleton()
		         //
		         .Then.Start<ICreateRequest>()
		         .Forward<CreateRequest<T>>()
		         .Include(x => x.Dependencies.Recursive())
		         .Singleton()
		         //
		         .Then.Start<INew<T>>()
		         .Forward<New<T>>()
		         .Singleton()
		         //
		         .Then.Start<ICreateExternal<T>>()
		         .Forward<CreateExternal<T>>()
		         .Decorate<LoggingAwareCreateExternal<T>>()
		         .Decorate<SynchronizationAwareCreateExternal<T>>()
		         .Include(x => x.Dependencies)
		         .Singleton()
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