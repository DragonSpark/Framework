using DragonSpark.Application.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Application
{
	public static class Extensions
	{
		/*public static EntityConfigurationContext<T> StoreEntitiesUsing<T>(this IServiceCollection @this)
			where T : DbContext
			=> new EntityConfigurationContext<T>(@this);*/

		public static IdentityContext<T> WithIdentity<T>(this IServiceCollection @this)
			where T : IdentityUser => @this.WithIdentity<T>(options => {});

		public static IdentityContext<T> WithIdentity<T>(this IServiceCollection @this,
		                                                 Action<IdentityOptions> configure)
			where T : IdentityUser
			=> new IdentityContext<T>(@this, configure);

		public static string UniqueId(this ExternalLoginInfo @this) => Security.UniqueId.Default.Get(@this);
	}
}