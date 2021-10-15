using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Model.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Security.Identity;

public sealed class ApplicationRegistrations<TContext, T> : CompositeCommand<IServiceCollection>
	where TContext : DbContext
	where T : IdentityUser
{
	public static ApplicationRegistrations<TContext, T> Default { get; } = new();

	ApplicationRegistrations() : base(Registrations<T>.Default, Claims.Registrations.Default,
	                                  Model.Registrations<T>.Default, Profile.Registrations<T>.Default,
	                                  DefaultRegistrations<TContext, T>.Default,
	                                  CommonRegistrations<TContext, T>.Default) {}
}