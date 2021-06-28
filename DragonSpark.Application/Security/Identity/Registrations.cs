using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity
{
	public sealed class Registrations<T> : CompositeCommand<IServiceCollection> where T : IdentityUser
	{
		public static Registrations<T> Default { get; } = new Registrations<T>();

		Registrations() : base(Authentication.Registrations<T>.Default, Claims.Registrations.Default,
		                       Model.Registrations<T>.Default, Profile.Registrations<T>.Default,
		                       DefaultRegistrations<T>.Default) {}
	}
}