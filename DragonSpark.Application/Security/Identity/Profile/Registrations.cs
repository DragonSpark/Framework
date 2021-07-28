using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.Profile
{
	sealed class Registrations<T> : ICommand<IServiceCollection> where T : IdentityUser
	{
		public static Registrations<T> Default { get; } = new Registrations<T>();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.Start<IUserSynchronization>()
			         .Forward<UserSynchronization<T>>()
			         .Scoped()
			         //
			         .Then.Start<IUserSynchronizer<T>>()
			         .Forward<UserSynchronizer<T>>()
			         .Scoped()
			         //
			         .Then.Start<ICreateRequest>()
			         .Forward<CreateRequest<T>>()
			         .Scoped()
					 //
			         .Then.Start<INew<T>>()
			         .Forward<New<T>>().Scoped()
			         //
			         .Then.Start<ICreate<T>>()
			         .Forward<Create<T>>()
			         .Decorate<LoggingAwareCreate<T>>()
			         .Scoped()
			         //
			         .Then.Start<ICreated<T>>()
			         .Forward<Created<T>>()
			         .Decorate<AddLoginAwareCreated<T>>()
			         .Decorate<SynchronizationAwareCreated<T>>()
			         .Scoped()
			         //
			         .Then.Start<ILocateUser<T>>()
			         .Forward<LocateUser<T>>()
			         .Scoped()
				;
		}
	}
}