using DragonSpark.Application.Compose;
using DragonSpark.Application.Compose.Entities;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace DragonSpark.Application
{
	public static class Extensions
	{
		public static IdentityContext<T> WithIdentity<T>(this ApplicationProfileContext @this)
			where T : IdentityUser => @this.WithIdentity<T>(options => {});

		public static IdentityContext<T> WithIdentity<T>(this ApplicationProfileContext @this,
		                                                 Action<IdentityOptions> configure)
			where T : IdentityUser
			=> new IdentityContext<T>(@this, configure);

		public static string UniqueId(this ExternalLoginInfo @this) => Security.Identity.UniqueId.Default.Get(@this);

		public static string Value(this ModelBindingContext @this, IResult<string> key)
			=> @this.ValueProvider.Get(key);

		public static string Get(this IValueProvider @this, IResult<string> key)
		{
			var name   = key.Get();
			var value  = @this.GetValue(name);
			var result = value != ValueProviderResult.None ? value.FirstValue : null;
			return result;
		}
	}
}