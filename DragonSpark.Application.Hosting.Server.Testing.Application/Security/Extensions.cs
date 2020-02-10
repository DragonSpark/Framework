using DragonSpark.Application.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Application.Hosting.Server.Testing.Application.Security
{
	public static class Extensions
	{
		public static IServiceCollection Memory<T, TUser>(this EntityStorageConfigurationContext<T, TUser> @this)
			where T : IdentityDbContext<TUser>
			where TUser : IdentityUser
			=> @this.Configuration(InMemoryStorageConfiguration.Default);
	}
}