using DragonSpark.Model.Commands;
using LightInject;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity
{
	public sealed class Registrations<TContext, T> : CompositeCommand<IServiceCollection>
		where TContext : DbContext
		where T : IdentityUser
	{
		public static Registrations<TContext, T> Default { get; } = new ();

		Registrations() : base(Authentication.Registrations<T>.Default, Claims.Registrations.Default,
		                       Model.Registrations<T>.Default, Profile.Registrations<T>.Default,
		                       DefaultRegistrations<TContext, T>.Default) {}
	}

	sealed class Compose : ICommand<IServiceContainer>
	{
		public static Compose Default { get; } = new();

		Compose() {}

		public void Execute(IServiceContainer parameter)
		{
			parameter.Decorate(typeof(IUsers<>), typeof(AmbientAwareUsers<>));
		}
	}

}