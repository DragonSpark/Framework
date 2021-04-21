using DragonSpark.Application.Components.Validation.Expressions;
using DragonSpark.Application.Compose;
using DragonSpark.Application.Runtime;
using DragonSpark.Application.Security;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using Humanizer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application
{
	partial class Extensions
	{
		public static BuildHostContext WithFrameworkConfigurations(this BuildHostContext @this)
			=> Configure.Default.Get(@this);

		public static BuildHostContext WithConnectionConfigurations(this BuildHostContext @this)
			=> Connections.Configure.Default.Get(@this);

		public static ApplicationProfileContext Apply(this BuildHostContext @this, IApplicationProfile profile)
			=> new ApplicationProfileContext(@this, profile);
/**/

		public static async ValueTask<T> Latest<T>(this DbContext @this, T entity) where T : notnull
		{
			await @this.Entry(entity).ReloadAsync().ConfigureAwait(false);
			return entity;
		}

		public static IQueryable<TEntity> Includes<TEntity>(this IQueryable<TEntity> source, params string[] includes)
			where TEntity : class => includes.Aggregate(source, (entities, include) => entities.Include(include));

		/**/

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

		public static string UserName(this ClaimsPrincipal @this, string anonymous) => @this.Identity?.Name ?? anonymous;

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

		public static Array<T> ApplyOrder<T>(this Array<T> @this) where T : class, IOrderAware
			=> Ordered<T>.Default.Get(@this);

		public static SelectedCollection<T> ToSelectedCollection<T>(this IEnumerable<T> @this) where T : class
			=> new SelectedCollection<T>(@this);

		/**/

		public static ConfiguredValueTaskAwaitable Await<T>(this IOperation<(Type Owner, Exception Exception)> @this,
		                                                    Exception exception)
			=> @this.Await(A.Type<T>(), exception);

		public static string Format<T>(this IResult<string> @this, T parameter) where T : notnull
			=> @this.Get().FormatWith(parameter.ToString());

		public static string Format<T1, T2>(this IResult<string> @this, (T1,T2) parameter)
			=> @this.Get().FormatWith(parameter.Item1, parameter.Item2);

		public static string Format(this IResult<string> @this, params object[] arguments)
			=> @this.Get().FormatWith(arguments);

		/**/

		public static (Type Owner, string Name) Key(this FieldIdentifier @this)
			=> (@this.Model.GetType(), @this.FieldName);

		/**/

		public static IValidateValue<object> Validator(this IExpression @this)
			=> new RegularExpressionValidator(@this.Get());

		public static BoundedExpression Bounded(this IExpression @this) => new BoundedExpression(@this.Get());

		public static IValidatingValue<T> Adapt<T>(this IValidateValue<T> @this)
			=> new ValidatingValueAdapter<T>(@this);

		/**/

		public static bool TryPop<T>(this IScopedVariable<T> @this, out T? element)
		{
			var result = @this.IsSatisfiedBy();
			element = result ? @this.Get() : default;
			@this.Remove.Execute();
			return result;
		}

	}
}
