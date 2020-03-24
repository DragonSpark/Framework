using DragonSpark.Application.Compose;
using DragonSpark.Application.Compose.Entities;
using DragonSpark.Composition.Compose;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Testing.Server
{
	public static class ExtensionMethods
	{
		public static BuildHostContext WithTestServer(this BuildHostContext @this)
			=> @this.Configure(ServerConfiguration.Default);

		public static ApplicationProfileContext Memory<T, TUser>(this EntityStorageConfigurationContext<T, TUser> @this)
			where T : Application.Security.Identity.IdentityDbContext<TUser>
			where TUser : IdentityUser
			=> @this.Configuration(InMemoryStorageConfiguration.Default);
	}
}