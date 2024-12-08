using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.State;

public sealed class PostConfigureCookieAuthenticationOptions : IPostConfigureOptions<CookieAuthenticationOptions>
{
	public void PostConfigure(string? name, CookieAuthenticationOptions options)
	{
		options.Events.OnCheckSlidingExpiration = x =>
		                                          {
			                                          x.ShouldRenew = false;
			                                          return Task.CompletedTask;
		                                          };
		options.Events.OnValidatePrincipal = x =>
		                                     {
			                                     x.ShouldRenew = false;
			                                     return Task.CompletedTask;
		                                     };
	}
}