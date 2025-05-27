using System.Net.Http;
using DragonSpark.Application.Communication.Http;
using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace DragonSpark.Application.AspNet.Communication.Http;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() {}

	public void Execute(IServiceCollection parameter)
	{
		parameter.AddTransient<DelegatingHandler, BearerHandler>()
		         .AddTransient<HttpMessageHandler, HttpClientHandler>()
		         //
		         .Start<IHttpContentSerializer>()
		         .Use<DefaultSerializer>()
		         .Singleton()
		         //
		         .Then.AddSingleton<ComposeAccessTokenView>()
		         .Start<IAccessTokenStore>()
		         .Forward<AccessTokenStore>()
		         .Include(x => x.Dependencies)
		         .Scoped()
		         .Then.TryDecorate<IAccessTokenProvider, AccessTokenProvider>();
	}
}