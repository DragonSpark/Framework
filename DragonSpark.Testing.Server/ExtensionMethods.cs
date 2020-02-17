using DragonSpark.Application.Compose;
using DragonSpark.Application.Compose.Entities;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Composition.Compose;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DragonSpark.Testing.Server
{
	public static class ExtensionMethods
	{
		public static BuildHostContext WithTestServer(this BuildHostContext @this)
			=> @this.Configure(ServerConfiguration.Default);

		public static ApplicationProfileContext Memory<T, TUser>(this EntityStorageConfigurationContext<T, TUser> @this)
			where T : IdentityDbContext<TUser>
			where TUser : IdentityUser
			=> @this.Configuration(InMemoryStorageConfiguration.Default)
			        .Configure(x => x.Decorate<T>((_, y) => y.With(z => z.Database.EnsureCreated())));
	}
}