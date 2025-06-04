using System;
using System.Collections.Generic;
using System.Security.Claims;
using DragonSpark.Application.Components.Validation.Expressions;
using DragonSpark.Application.Compose;
using DragonSpark.Application.Compose.Runtime;
using DragonSpark.Application.Compose.Store.Operations.Memory;
using DragonSpark.Application.Model.Sequences;
using DragonSpark.Application.Runtime;
using DragonSpark.Application.Runtime.Operations.Execution;
using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Humanizer;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Application;

partial class Extensions
{
	public static IHostBuilder WithAmbientConfiguration(this IHostBuilder @this)
		=> @this.To(Configuration.WithAmbientConfiguration.Default);

	public static BuildHostContext WithAmbientConfiguration(this BuildHostContext @this)
		=> @this.Select(Configuration.WithAmbientConfiguration.Default);

	/**/
	public static BuildHostContext WithHttp(this BuildHostContext @this)
		=> @this.Configure(Communication.Http.Registrations.Default);

	public static bool HasClaim(this ClaimsPrincipal @this, string claim) => @this.HasClaim(x => x.Type == claim);

	public static Claim Claim(this Text.Text @this, string value) => new(@this, value);

	public static bool IsAuthenticated(this ClaimsPrincipal @this) => @this.Identity?.IsAuthenticated ?? false;

	public static T Get<T>(this ISelect<ClaimsIdentity, T> @this, ClaimsPrincipal parameter)
		=> @this.Get(PrincipalIdentity.Default.Get(parameter));

	/**/

	public static ICollection<T> OrderedLarge<T>(this ICollection<T> @this) where T : class, ILargeOrderAware
		=> LargeOrdered<T>.Default.Parameter(@this).Return(@this);

	public static ICollection<T> Ordered<T>(this ICollection<T> @this) where T : class, IOrderAware
		=> Runtime.Ordered<T>.Default.Parameter(@this).Return(@this);

	public static SelectedCollection<T> ToSelectedCollection<T>(this IEnumerable<T> @this) where T : class
		=> new(@this);

	public static TList AddRange<TList, T>(this TList @this, Memory<T> range) where TList : List<T>
		=> CopyList<TList, T>.Default.Get(new(range, @this));

	public static ICollection<T> Rebuild<T>(this ICollection<T> @this, Memory<T> source)
		=> Compose.Runtime.Rebuild<T>.Default.Get(new(@this, source));

	/**/

	public static string Format<T>(this IResult<string> @this, T parameter) where T : notnull
		=> @this.Get().FormatWith(parameter.ToString());

	public static string Format<T1, T2>(this IResult<string> @this, (T1, T2) parameter)
		=> @this.Get().FormatWith(parameter.Item1, parameter.Item2);

	public static string Format(this IResult<string> @this, params object[] arguments)
		=> @this.Get().FormatWith(arguments);

	public static string Smart<T>(this IResult<string> @this, T parameter) where T : notnull
		=> SmartFormat.Smart.Format(@this.Get(), parameter);

	/**/

	public static IValidateValue<object> Validator(this IExpression @this)
		=> new RegularExpressionValidator(@this.Get());

	public static BoundedExpression Bounded(this IExpression @this) => new(@this.Get());

	public static IValidatingValue<T> Adapt<T>(this IValidateValue<T> @this)
		=> new ValidatingValueAdapter<T>(@this);

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
	public static string Ordinalize(this in byte @this) => ((int)@this).Ordinalize();

	public static string Ordinalize(this in ushort @this) => ((int)@this).Ordinalize();

	public static string Ordinalize(this in uint @this) => ((int)@this).Ordinalize();

/**/
	public static OperationComposer<T> Then<T>(this DragonSpark.Compose.Model.Operations.OperationComposer<T> @this)
		=> new(@this.Get());

/**/

	public static IOperation<T> Ambient<T>(this IOperation<T> @this) => new DeferredOperation<T>(@this);

/**/
	public static DragonSpark.Compose.Model.Operations.OperationResultComposer<TIn, TOut> Using<TIn, TOut>(
		this Compose.Store.Operations.StoreContext<TIn, TOut> @this, StoreProfile<TIn> profile)
		=> @this.In(profile.Memory).For(profile.For).Using(profile.Key);
}