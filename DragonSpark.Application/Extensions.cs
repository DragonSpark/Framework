using DragonSpark.Application.Components.Validation.Expressions;
using DragonSpark.Application.Compose;
using DragonSpark.Application.Compose.Runtime;
using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using DragonSpark.Application.Model.Sequences;
using DragonSpark.Application.Runtime;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Application.Security.Identity.Claims.Access;
using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Results;
using Humanizer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application;

partial class Extensions
{
	public static BuildHostContext WithFrameworkConfigurations(this BuildHostContext @this)
		=> Configure.Default.Get(@this);

	public static BuildHostContext WithConnectionConfigurations(this BuildHostContext @this)
		=> Connections.Configure.Default.Get(@this);

	public static BuildHostContext WithClientConnectionConfigurations(this BuildHostContext @this)
		=> Connections.Client.Configure.Default.Get(@this);

	public static BuildHostContext WithAmbientConfiguration(this BuildHostContext @this)
		=> @this.Select(Configuration.WithAmbientConfiguration.Default);

	public static ApplicationProfileContext Apply(this BuildHostContext @this, IApplicationProfile profile)
		=> new(@this, profile);

	public static ApplicationProfileContext WithIdentityClaimsRelay(this ApplicationProfileContext @this)
		=> @this.Append(Security.Identity.Authentication.Persist.WithIdentityClaimsRelay.Default);

	/**/

	public static string ValueOrDefault(this Accessed @this) => @this.ValueOrDefault(string.Empty);

	public static string ValueOrDefault(this Accessed @this, string @default)
		=> @this.Exists ? @this.Value.Verify() : @default;

	public static string Value(this Accessed @this)
		=> @this.Exists ? @this.Value.Verify() : throw new InvalidOperationException($"{@this.Claim} not found.");

	public static bool HasClaim(this ClaimsPrincipal @this, string claim) => @this.HasClaim(x => x.Type == claim);

	public static Claim Claim(this Text.Text @this, string value) => new(@this, value);

	public static string DisplayName(this ClaimsPrincipal @this) => @this.DisplayName(Anonymous.Default);

	public static string DisplayName(this ClaimsPrincipal @this, string anonymous)
		=> @this.FindFirstValue(Security.Identity.DisplayName.Default) ?? @this.UserName(anonymous);

	public static string UserName(this ClaimsPrincipal @this) => @this.UserName(Anonymous.Default);

	public static string UserName(this ClaimsPrincipal @this, string anonymous)
		=> @this.Identity?.Name ?? anonymous;

	public static string? Get(this IValueProvider @this, string key)
	{
		var value  = @this.GetValue(key);
		var result = value != ValueProviderResult.None ? value.FirstValue : null;
		return result;
	}

	public static T User<T>(this AuthenticationState @this) where T : IdentityUser
		=> @this.To<AuthenticationState<T>>().Profile.Verify();

	public static ProviderIdentity AsIdentity(this ExternalLoginInfo @this)
		=> ExternalLoginIdentity.Default.Get(@this);

	/**/

	public static ICollection<T> OrderedLarge<T>(this ICollection<T> @this) where T : class, ILargeOrderAware
		=> LargeOrdered<T>.Default.Parameter(@this).Return(@this);

	public static ICollection<T> Ordered<T>(this ICollection<T> @this) where T : class, IOrderAware
		=> Runtime.Ordered<T>.Default.Parameter(@this).Return(@this);

	public static SelectedCollection<T> ToSelectedCollection<T>(this IEnumerable<T> @this) where T : class
		=> new(@this);

	public static TList AddRange<TList, T>(this TList @this, Memory<T> range) where TList : List<T>
		=> CopyList<TList, T>.Default.Get(new(range, @this));

	/**/

	public static string Format<T>(this IResult<string> @this, T parameter) where T : notnull
		=> @this.Get().FormatWith(parameter.ToString());

	public static string Format<T1, T2>(this IResult<string> @this, (T1, T2) parameter)
		=> @this.Get().FormatWith(parameter.Item1, parameter.Item2);

	public static string Format(this IResult<string> @this, params object[] arguments)
		=> @this.Get().FormatWith(arguments);

	/**/

	public static (Type Owner, string Name) Key(this FieldIdentifier @this)
		=> (@this.Model.GetType(), @this.FieldName);

	/**/

	public static IValidateValue<object> Validator(this IExpression @this)
		=> new RegularExpressionValidator(@this.Get());

	public static BoundedExpression Bounded(this IExpression @this) => new(@this.Get());

	public static IValidatingValue<T> Adapt<T>(this IValidateValue<T> @this)
		=> new ValidatingValueAdapter<T>(@this);

	/**/

	public static Assignment<T> Assigned<T>(this IMutable<T?> @this, T value)
	{
		@this.Pass(value);
		return new(@this);
	}

	public static bool TryPop<T>(this IMutable<T?> @this, out T? element)
	{
		element = @this.Get();
		@this.Execute(default);
		return element is not null;
	}

	public static bool IfPop<T>(this IMutable<T?> @this, T @if)
	{
		var stored = @this.Get();
		var result = EqualityComparer<T?>.Default.Equals(@if, stored);
		if (result)
		{
			@this.Execute(default);
		}
		return result;
	}

	public static Task<int> Save(this DbContext @this) => @this.SaveChangesAsync();

	/**/
	public static bool HasResults<T>(this IPages<T> @this) => @this != EmptyPages<T>.Default;
	public static bool IsEmpty<T>(this IPages<T> @this) => @this == EmptyPages<T>.Default;

/**/
	public static string Ordinalize(this in byte @this) => ((int)@this).Ordinalize();
	public static string Ordinalize(this in ushort @this) => ((int)@this).Ordinalize();
	public static string Ordinalize(this in uint @this) => ((int)@this).Ordinalize();
}