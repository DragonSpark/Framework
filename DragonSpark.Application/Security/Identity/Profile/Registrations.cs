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
		parameter.Start<IUserSynchronizer<T>>().Forward<UserSynchronizer<T>>().Singleton();
	}
}