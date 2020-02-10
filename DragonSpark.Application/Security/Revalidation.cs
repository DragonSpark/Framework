using DragonSpark.Compose;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security
{
	public sealed class Revalidation<T> : RevalidatingServerAuthenticationStateProvider where T : class
	{
		readonly IServiceScopeFactory _scopes;
		readonly IdentityOptions      _options;

		// ReSharper disable once TooManyDependencies
		public Revalidation(ILoggerFactory loggerFactory, IServiceScopeFactory scopes, IOptions<IdentityOptions> options)
			: this(loggerFactory, scopes, options.Value, TimeSpan.FromMinutes(30)) {}

		// ReSharper disable once TooManyDependencies
		public Revalidation(ILoggerFactory loggerFactory, IServiceScopeFactory scopes, IdentityOptions options,
		                   TimeSpan interval)
			: base(loggerFactory)
		{
			_scopes              = scopes;
			_options             = options;
			RevalidationInterval = interval;
		}

		protected override TimeSpan RevalidationInterval { get; }

		protected override async Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState,
		                                                                     CancellationToken cancellationToken)
		{
			// Get the user manager from a new scope to ensure it fetches fresh data
			var scope = _scopes.CreateScope();
			try
			{
				var userManager = scope.ServiceProvider.GetRequiredService<UserManager<T>>();
				return await ValidateSecurityStampAsync(userManager, authenticationState.User);
			}
			finally
			{
				await scope.Disposed();
			}
		}

		async Task<bool> ValidateSecurityStampAsync(UserManager<T> users, ClaimsPrincipal principal)
		{
			var user = await users.GetUserAsync(principal);
			if (user != null)
			{
				var result = !users.SupportsUserSecurityStamp ||
				             principal.FindFirstValue(_options.ClaimsIdentity.SecurityStampClaimType) ==
				             await users.GetSecurityStampAsync(user);
				return result;
			}

			return false;
		}
	}
}
