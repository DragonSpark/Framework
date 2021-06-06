using DragonSpark.Application.Compose.Entities;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Composition.Compose;
using Microsoft.EntityFrameworkCore;

namespace DragonSpark.Testing.Server
{
	public static class ExtensionMethods
	{
		public static BuildHostContext WithTestServer(this BuildHostContext @this)
			=> @this.Configure(ServerConfiguration.Default);

		public static ConfiguredIdentityStorage<T, TContext> Memory<T, TContext>(
			this IdentityStorageConfiguration<T, TContext> @this)
			where T : IdentityUser
			where TContext : DbContext
			=> @this.Configuration(InMemoryStorageConfiguration.Default);
	}
}