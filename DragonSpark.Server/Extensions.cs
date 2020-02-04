using DragonSpark.Composition.Compose;
using DragonSpark.Server.Application;
using DragonSpark.Server.Compose;
using DragonSpark.Server.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Server
{
	public static class Extensions
	{
		public static ServerProfileContext Apply(this BuildHostContext @this, IServerProfile profile)
			=> new ServerProfileContext(@this, profile);

		public static EntityConfigurationContext<T> StoreEntitiesUsing<T>(this IServiceCollection @this)
			where T : DbContext
			=> new EntityConfigurationContext<T>(@this);
	}
}