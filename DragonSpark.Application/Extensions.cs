using DragonSpark.Application.Compose;
using DragonSpark.Application.Compose.Entities;
using DragonSpark.Application.Security;
using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application
{
	public static class Extensions
	{
		public static T Undo<T>(this DbContext @this, T entity) where T : class
		{
			var entry = @this.Entry(entity);
			entry.CurrentValues.SetValues(entry.OriginalValues);
			entry.State = EntityState.Unchanged;
			return entity;
		}

		public static IdentityContext WithIdentity(this ApplicationProfileContext @this)
			=> @this.WithIdentity(options => {});

		public static IdentityContext WithIdentity(this ApplicationProfileContext @this,
		                                           Action<IdentityOptions> configure)
			=> new IdentityContext(@this, configure);

		public static AuthenticationContext WithAuthentication(this ApplicationProfileContext @this)
			=> new AuthenticationContext(@this);

		public static ApplicationProfileContext AuthorizeUsing(this ApplicationProfileContext @this,
		                                                       ICommand<AuthorizationOptions> policy)
			=> @this.AuthorizeUsing(policy.Execute);

		public static ApplicationProfileContext AuthorizeUsing(this ApplicationProfileContext @this,
		                                                       Action<AuthorizationOptions> policy)
			=> @this.Then(new AuthorizeConfiguration(policy));

		public static string UniqueId(this ExternalLoginInfo @this) => Security.Identity.UniqueId.Default.Get(@this);

		public static ProviderIdentity Identity(this IdentityUser @this) => Identities.Default.Get(@this.UserName);

		public static async ValueTask<T> GetUser<T>(this UserManager<T> @this, ExternalLoginInfo login)
			where T : IdentityUser
		{
			var id     = login.UniqueId();
			var result = await @this.Users.SingleAsync(x => x.UserName == id);
			return result;
		}

		public static bool HasClaim(this ClaimsPrincipal @this, IResult<string> claim)
		{
			var type   = claim.Get();
			var result = @this.HasClaim(x => x.Type == type);
			return result;
		}

		/*public static Claim Claim(this Text.Text @this, Claim claim) => @this.Claim(claim.Value);*/

		public static Claim Claim(this Text.Text @this, string value) => new Claim(@this, value);

		public static IUserMapping Promote<T>(this IAccessor<T> @this, string key, bool required = false)
			where T : IdentityUser
			=> new UserMapping<T>(@this, key, required);


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