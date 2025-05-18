using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Application.AspNet.Entities.Diagnostics;
using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Application.AspNet.Entities.Transactions;
using DragonSpark.Application.AspNet.Model.Content;
using DragonSpark.Application.AspNet.Security.Identity;
using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Application.Security.Identity.Claims;
using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IdentityUser = DragonSpark.Application.AspNet.Security.Identity.IdentityUser;

namespace DragonSpark.Application.AspNet;

partial class Extensions
{
	public static BuildHostContext WithFrameworkConfigurations(this BuildHostContext @this)
		=> Configure.Default.Get(@this);

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

	public static Claim? Claim(this Accessed @this) => @this.Exists ? new(@this.Claim, @this.Value.Verify()) : null;


	public static uint? Number(this ClaimsPrincipal @this) => UserNumber.Default.Get(@this);

	public static ProviderIdentity AuthenticatedIdentity(this ClaimsPrincipal @this)
		=> Security.Identity.AuthenticatedIdentity.Default.Get(@this);

	public static ProviderIdentity Identity(this ClaimsPrincipal @this) => Identities.Default.Get(@this);

	public static string DisplayName(this ClaimsPrincipal @this) => UserDisplayName.Default.Get(@this);

	public static string UserName(this ClaimsPrincipal @this) => Security.Identity.UserName.Default.Get(@this);

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

	public static T Get<T>(this ISelect<ClaimsPrincipal, T> @this, AuthenticationState parameter)
		=> @this.Get(parameter.User);

	/**/

	public static (Type Owner, string Name) Key(this FieldIdentifier @this)
		=> (@this.Model.GetType(), @this.FieldName);

	/**/

	public static string? Read(this IReadClaim @this, ClaimsPrincipal parameter)
		=> @this.Get(parameter).To<Accessed, string?>(x => x.Exists ? x.Value : null);

	public static Task Save(this DbContext @this, CancellationToken stop) => @this.SaveChangesAsync(stop);

	public static T Attached<T>(this IEditor @this, T parameter) where T : class
	{
		@this.Attach(parameter);
		return parameter;
	}

	/**/

	public static MarkupString AsMarkup(this string? @this) => AsMarkdown.Default.Get(@this);

	/**/

	public static ITransactions Ambient(this ITransactions @this) => new AmbientAwareTransactions(@this);

	public static IOperation<T> ReloadAware<T>(this IOperation<T> @this) => new ReloadAware<T>(@this);
}