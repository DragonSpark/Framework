using DragonSpark.Application.Compose;
using DragonSpark.Application.Entities.Diagnostics;
using DragonSpark.Application.Entities.Transactions;
using DragonSpark.Application.Runtime.Operations.Execution;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Application.Security.Identity.Claims.Access;
using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
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

	/*public static IHostBuilder WithAmbientConfiguration(this IHostBuilder @this)
		=> @this.To(Configuration.WithAmbientConfiguration.Default);

	public static BuildHostContext WithAmbientConfiguration(this BuildHostContext @this)
		=> @this.Select(Configuration.WithAmbientConfiguration.Default);*/

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

	/*
	public static bool HasClaim(this ClaimsPrincipal @this, string claim) => @this.HasClaim(x => x.Type == claim);

	public static Claim Claim(this Text.Text @this, string value) => new(@this, value);

	public static uint? Number(this ClaimsPrincipal @this) => UserNumber.Default.Get(@this);

	/*public static string? Identifier(this ClaimsPrincipal @this) => UserIdentifier.Default.Get(@this);#1#

	public static ProviderIdentity AuthenticatedIdentity(this ClaimsPrincipal @this)
		=> Security.Identity.AuthenticatedIdentity.Default.Get(@this);

	public static ProviderIdentity Identity(this ClaimsPrincipal @this) => Identities.Default.Get(@this);

	public static string DisplayName(this ClaimsPrincipal @this) => UserDisplayName.Default.Get(@this);

	public static string UserName(this ClaimsPrincipal @this) => Security.Identity.UserName.Default.Get(@this);

	public static bool IsAuthenticated(this ClaimsPrincipal @this) => @this.Identity?.IsAuthenticated ?? false;

	*/
	public static T Get<T>(this ISelect<ClaimsIdentity, T> @this, ClaimsPrincipal parameter)
		=> @this.Get(PrincipalIdentity.Default.Get(parameter));
	public static uint? Number(this ClaimsPrincipal @this) => UserNumber.Default.Get(@this);

	/*public static string? Identifier(this ClaimsPrincipal @this) => UserIdentifier.Default.Get(@this);*/

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

	/*public static IValidateValue<object> Validator(this IExpression @this)
		=> new RegularExpressionValidator(@this.Get());

	public static BoundedExpression Bounded(this IExpression @this) => new(@this.Get());

	public static IValidatingValue<T> Adapt<T>(this IValidateValue<T> @this)
		=> new ValidatingValueAdapter<T>(@this);*/

	/**/

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
/**/
	public static string? Read(this IReadClaim @this, ClaimsPrincipal parameter)
		=> @this.Get(parameter).To<Accessed, string?>(x => x.Exists ? x.Value : null);

	public static Task<int> Save(this DbContext @this) => @this.SaveChangesAsync();


/**/
	/*public static string Ordinalize(this in byte @this) => ((int)@this).Ordinalize();

	public static string Ordinalize(this in ushort @this) => ((int)@this).Ordinalize();

	public static string Ordinalize(this in uint @this) => ((int)@this).Ordinalize();*/

/**/
	/*public static OperationComposer<T> Then<T>(this DragonSpark.Compose.Model.Operations.OperationComposer<T> @this)
		=> new(@this.Get());*/

/**/
	public static ITransactions Ambient(this ITransactions @this) => new AmbientAwareTransactions(@this);

	public static IOperation<T> Ambient<T>(this IOperation<T> @this) => new DeferredOperation<T>(@this);

	public static IOperation<T> ReloadAware<T>(this IOperation<T> @this) => new ReloadAware<T>(@this);

/**/
	/*public static DragonSpark.Compose.Model.Operations.OperationResultComposer<TIn, TOut> Using<TIn, TOut>(
		this Compose.Store.Operations.StoreContext<TIn, TOut> @this, StoreProfile<TIn> profile)
		=> @this.In(profile.Memory).For(profile.For).Using(profile.Key);*/
}