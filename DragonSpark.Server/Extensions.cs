using DragonSpark.Application.AspNet;
using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model;
using DragonSpark.Server.Requests;
using DragonSpark.Server.Security.Content;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace DragonSpark.Server;

public static class Extensions
{
	public static ApplicationProfileContext WithContentSecurity(this ApplicationProfileContext @this)
		=> @this.Append(x => x.AddContentSecurity()).Append(x => x.UseContentSecurity());

	extension(BuildHostContext @this)
	{
		public BuildHostContext WithOutputs() => @this.Configure(Output.Registrations.Default);

		public BuildHostContext WithContentSecurity() => @this.Configure(Registrations.Default);

		public BuildHostContext WithChallenges() => @this.Configure(Security.Challenges.Registrations.Default);
	}

	public static IServiceCollection AddContentSecurity(this IServiceCollection @this)
		=> Registrations.Default.Parameter(@this);

	public static IApplicationBuilder UseContentSecurity(this IApplicationBuilder @this)
		=> @this.UseMiddleware<ApplyPolicy>();

	/**/

	extension(Controller @this)
	{
		public View NewView(Guid subject) => new (@this, subject);

		public View<T> NewView<T>(T subject) => new (@this, subject);
	}

	extension(ControllerBase @this)
	{
		public Request<None> New(Guid identity) => @this.New(identity, None.Default);

		public Request<T> New<T>(Guid identity, T subject)
			=> new(@this, new(@this.User.Number(), identity, subject));

		public Query Query(Guid subject) => new(@this, subject);

		public Query<T> Query<T>(T subject) => new(@this, subject);
	}

	extension(ClaimsPrincipal @this)
	{
		public Input Input(Guid input) => new (@this, input);

		public Input<T> Input<T>(T input) => new (@this, input);
	}

	public static Query<TOther> Subject<T, TOther>(this @Query<T> @this, TOther subject) => new(@this.Owner, subject);
}