using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity.MultiFactor;

sealed class Registrations<T> : ICommand<IServiceCollection> where T : IdentityUser
{
	public static Registrations<T> Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Start<IKeyCode<T>>()
		         .Forward<KeyCode<T>>()
		         .Decorate<FormatAwareKeyCode<T>>()
		         .Singleton()
		         //
		         .Then.Start<IDisable<T>>()
		         .Forward<Disable<T>>()
		         .Singleton()
			;
	}
}