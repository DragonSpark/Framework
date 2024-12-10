using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

[MustDisposeResource]
sealed class Revalidation(ILoggerFactory loggers, IValidationServices validation, TimeSpan interval)
	: RevalidatingServerAuthenticationStateProvider(loggers)
{
	public Revalidation(ILoggerFactory loggers, IValidationServices validation)
		: this(loggers, validation, TimeSpan.FromMinutes(1)) {}

	protected override TimeSpan RevalidationInterval { get; } = interval;

	public override Task<AuthenticationState> GetAuthenticationStateAsync()
		=> validation.Get(base.GetAuthenticationStateAsync());

	protected override Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState,
	                                                               CancellationToken cancellationToken)
		=> validation.Get(authenticationState.User).AsTask();
}
