using DragonSpark.Application.AspNet.Entities.Diagnostics;
using DragonSpark.Application.AspNet.Entities.Editing;
using DragonSpark.Application.AspNet.Entities.Transactions;
using DragonSpark.Application.AspNet.Model.Content;
using DragonSpark.Application.AspNet.Security;
using DragonSpark.Application.AspNet.Security.Identity;
using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Application.AspNet.Workers;
using DragonSpark.Application.Model;
using DragonSpark.Application.Security.Identity.Claims;
using DragonSpark.Compose;
using DragonSpark.Contracts.Queries;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Claim = System.Security.Claims.Claim;
using IdentityUser = DragonSpark.Application.AspNet.Security.Identity.IdentityUser;

namespace DragonSpark.Application.AspNet;

partial class Extensions
{
	extension(Accessed @this)
	{
		public string ValueOrDefault() => @this.ValueOrDefault(string.Empty);

		public string ValueOrDefault(string @default)
			=> @this.Exists ? @this.Value.Verify() : @default;

		public string Value()
			=> @this.Exists ? @this.Value.Verify() : throw new InvalidOperationException($"{@this.Claim} not found.");

		public Claim? Claim() => @this.Exists ? new(@this.Claim, @this.Value.Verify()) : null;
	}

	extension(ClaimsPrincipal @this)
	{
		public uint? Number() => UserNumber.Default.Get(@this);

		public ProviderIdentity AuthenticatedIdentity()
			=> Security.Identity.AuthenticatedIdentity.Default.Get(@this);

		public ProviderIdentity Identity() => Identities.Default.Get(@this);

		public string DisplayName() => UserDisplayName.Default.Get(@this);

		public string UserName() => Security.Identity.UserName.Default.Get(@this);

		public string Name() => @this.Identity.Verify().Name.Verify();
	}

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

	public static IStopAware<T> ReloadAware<T>(this IStopAware<T> @this) => new ReloadAware<T>(@this);

	public static IStopAware<TIn, TOut> ReloadAware<TIn, TOut>(this IStopAware<TIn, TOut> @this)
		=> new ReloadAware<TIn, TOut>(@this);

	extension(ClaimsPrincipal @this)
	{
		/**/
		public UserInput Input(Guid subject) => new(@this.Number().Value(), subject);

		public UserInput<T> Input<T>(T subject) => new(@this.Number() ?? 0, subject);
	}

	extension(HttpContext @this)
	{
		public UserInput Input(Guid subject) => @this.User.Input(subject);

		public UserInput<T> Input<T>(T subject) => @this.User.Input(subject);

		public Stop<PageQueryInput<uint>> PagingUserInput(PageRequest page)
			=> @this.PagingInput(@this.User.Number().Value(), page);

		public Stop<PageQueryInput<UserInput>> PagingUserInput(Guid parameter,
															   PageRequest page)
			=> @this.PagingInput(new UserInput(@this.User.Number().Value(), parameter), page);

		public Stop<PageQueryInput<UserInput<T>>> PagingUserInput<T>(T parameter,
																	 PageRequest page)
			=> @this.PagingInput(new UserInput<T>(@this.User.Number().Value(), parameter), page);

		public Stop<PageQueryInput<T>> PagingInput<T>(T parameter, PageRequest page)
			=> new(new(parameter, page), @this.RequestAborted);

		public Stop<uint> UserInput()
			=> new(@this.User.Number().Value(), @this.RequestAborted);

		public Stop<T> Stop<T>(T parameter) => new(parameter, @this.RequestAborted);

		public Stop<UserInput<T>> UserInput<T>(T subject)
			=> new(@this.User.Input(subject), @this.RequestAborted);

		public Stop<UserInput> UserInput(Guid subject)
			=> new(@this.User.Input(subject), @this.RequestAborted);
	}

	/**/

	extension(NavigationManager @this)
	{
		public Task Navigate(string path, CancellationToken stop) => @this.Navigate(path, false, stop);

		public Task Navigate(string path, bool force, CancellationToken stop)
		{
			@this.NavigateTo(path, force);
			return Task.Delay(Timeout.Infinite, stop);
		}

		public bool IsOn(string parameter) => Navigation.IsOn.Default.Get(new(@this, parameter));

		public string RootPath() => Navigation.RootPath.Default.Get(@this);

		public string Path() => Navigation.Path.Default.Get(@this);
	}

	public static string Nonce(this HttpContext @this) => HttpContextNonce.Default.Get(@this);

	/**/
	public static T Update<T>(this T @this, ProcessUpdate parameter) where T : ExternalProcess
		=> UpdateProcess.Default.Parameter(new(@this, parameter)).Process.To<T>();
}