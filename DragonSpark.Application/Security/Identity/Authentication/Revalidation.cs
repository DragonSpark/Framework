using JetBrains.Annotations;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	sealed class Revalidation : RevalidatingServerAuthenticationStateProvider
	{
		readonly IValidationServices _validation;

		[UsedImplicitly]
		public Revalidation(ILoggerFactory loggerFactory, IValidationServices validation)
			: this(loggerFactory, validation, TimeSpan.FromMinutes(10)) {}

		public Revalidation(ILoggerFactory loggerFactory, IValidationServices validation, TimeSpan interval)
			: base(loggerFactory)
		{
			_validation          = validation;
			RevalidationInterval = interval;
		}

		protected override TimeSpan RevalidationInterval { get; }

		public override Task<AuthenticationState> GetAuthenticationStateAsync()
			=> _validation.Get(base.GetAuthenticationStateAsync());

		protected override Task<bool> ValidateAuthenticationStateAsync(AuthenticationState authenticationState,
		                                                               CancellationToken cancellationToken)
			=> _validation.Get(authenticationState.User).AsTask();
	}
}