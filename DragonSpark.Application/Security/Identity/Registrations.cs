using DragonSpark.Model.Commands;
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
}