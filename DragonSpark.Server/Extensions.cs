using DragonSpark.Application;
using DragonSpark.Application.Compose;
using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model;
using DragonSpark.Server.Requests;
using DragonSpark.Server.Security.Content;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Security.Claims;

namespace DragonSpark.Server;

public static class Extensions
{
	public static ApplicationProfileContext WithContentSecurity(this ApplicationProfileContext @this)
		=> @this.Append(x => x.AddContentSecurity()).Append(x => x.UseContentSecurity());

	public static BuildHostContext WithContentSecurity(this BuildHostContext @this)
		=> @this.Configure(Registrations.Default);

	public static IServiceCollection AddContentSecurity(this IServiceCollection @this)
		=> Registrations.Default.Parameter(@this);

	public static IApplicationBuilder UseContentSecurity(this IApplicationBuilder @this)
		=> @this.UseMiddleware<ApplyPolicy>();

	/**/

	public static View NewView(this Controller @this, Guid subject) => new (@this, subject);
	public static View<T> NewView<T>(this Controller @this, T subject) => new (@this, subject);

	public static Request<None> New(this ControllerBase @this, Guid identity) => @this.New(identity, None.Default);

	public static Request<T> New<T>(this ControllerBase @this, Guid identity, T subject)
		=> new(@this, new(@this.User.Number(), identity, subject));

	public static Query Query(this ControllerBase @this, Guid subject) => new(@this, subject);
	public static Query<T> Query<T>(this ControllerBase @this, T subject) => new(@this, subject);

	public static Input Input(this ClaimsPrincipal @this, Guid input) => new (@this, input);
	public static Input<T> Input<T>(this ClaimsPrincipal @this, T input) => new (@this, input);

	public static Query<TOther> Subject<T, TOther>(this @Query<T> @this, TOther subject)
		=> new Query<TOther>(@this.Owner, subject);
}