using DragonSpark.Application.Compose;
using DragonSpark.Application.Compose.Entities;
using DragonSpark.Application.Compose.Store;
using DragonSpark.Application.Runtime;
using DragonSpark.Application.Security;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Compose;
using DragonSpark.Compose.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application
{
	public static class Extensions
	{
		public static async ValueTask<T> Latest<T>(this DbContext @this, T entity) where T : notnull
		{
			await @this.Entry(entity).ReloadAsync().ConfigureAwait(false);
			return entity;
		}

		public static IQueryable<TEntity> Includes<TEntity>(this IQueryable<TEntity> source, params string[] includes)
			where TEntity : class => includes.Aggregate(source, (entities, include) => entities.Include(include));

		/**/

		public static IdentityContext WithIdentity(this ApplicationProfileContext @this)
			=> @this.WithIdentity(options => {});

		public static IdentityContext WithIdentity(this ApplicationProfileContext @this,
		                                           System.Action<IdentityOptions> configure)
			=> new IdentityContext(@this, configure);

		public static AuthenticationContext WithAuthentication(this ApplicationProfileContext @this)
			=> new AuthenticationContext(@this);

		public static ApplicationProfileContext AuthorizeUsing(this ApplicationProfileContext @this,
		                                                       ICommand<AuthorizationOptions> policy)
			=> @this.AuthorizeUsing(policy.Execute);

		public static ApplicationProfileContext AuthorizeUsing(this ApplicationProfileContext @this,
		                                                       System.Action<AuthorizationOptions> policy)
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

		public static Claim Claim(this Text.Text @this, string value) => new Claim(@this, value);

		public static string DisplayName(this ClaimsPrincipal @this) => @this.DisplayName(Anonymous.Default);

		public static string DisplayName(this ClaimsPrincipal @this, string anonymous)
			=> @this.FindFirstValue(Security.Identity.DisplayName.Default) ?? @this.UserName(anonymous);

		public static string UserName(this ClaimsPrincipal @this) => @this.UserName(Anonymous.Default);

		public static string UserName(this ClaimsPrincipal @this, string anonymous) => @this.Identity.Name ?? anonymous;

		public static IUserMapping Promote<T>(this IAccessor<T> @this, string key, bool required = false)
			where T : IdentityUser
			=> new UserMapping<T>(@this, key, required);

		public static string? Value(this ModelBindingContext @this, IResult<string> key)
			=> @this.ValueProvider.Get(key);

		public static string? Get(this IValueProvider @this, IResult<string> key)
		{
			var name   = key.Get();
			var value  = @this.GetValue(name);
			var result = value != ValueProviderResult.None ? value.FirstValue : null;
			return result;
		}

		public static async Task<AuthenticationState<T>> Promote<T>(this Task<AuthenticationState> @this)
			where T : class
		{
			var state  = await @this;
			var result = state.To<AuthenticationState<T>>();
			return result;
		}

		public static T User<T>(this AuthenticationState @this) where T : class
			=> @this.To<AuthenticationState<T>>().Profile.Verify();

		/**/

		public static StoreContext<TIn, TOut> Store<TIn, TOut>(this Selector<TIn, TOut> @this)
			=> new StoreContext<TIn, TOut>(@this);

		public static Compose.Store.Operations.StoreContext<TIn, TOut> Store<TIn, TOut>(
			this OperationResultSelector<TIn, TOut> @this)
			=> new Compose.Store.Operations.StoreContext<TIn, TOut>(@this);

		public static Slide Slide(this TimeSpan @this) => new Slide(@this);

		/**/

		public static Array<T> ApplyOrder<T>(this Array<T> @this) where T : class, IOrderAware
			=> Ordered<T>.Default.Get(@this);
	}
}