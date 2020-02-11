using DragonSpark.Application.Entities;
using DragonSpark.Composition.Compose;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Testing.Server
{
	public static class ExtensionMethods
	{
		public static BuildHostContext WithTestServer(this BuildHostContext @this)
			=> @this.Configure(ServerConfiguration.Default);

		public static IServiceCollection Memory<T, TUser>(this EntityStorageConfigurationContext<T, TUser> @this)
			where T : IdentityDbContext<TUser>
			where TUser : IdentityUser
			=> @this.Configuration(InMemoryStorageConfiguration.Default);
	}
}