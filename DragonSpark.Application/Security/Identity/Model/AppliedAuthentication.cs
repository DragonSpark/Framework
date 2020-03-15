using DragonSpark.Application.Security.Identity.Profile;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Model {
	sealed class AppliedAuthentication : IAppliedAuthentication
	{
		readonly IAppliedPrincipal _principal;

		public AppliedAuthentication(IAppliedPrincipal principal) => _principal = principal;

		public ExternalLoginInfo Get(ExternalLoginInfo parameter)
			=> new ExternalLoginInfo(_principal.Get(parameter), parameter.LoginProvider, parameter.ProviderKey,
			                         parameter.ProviderDisplayName);
	}
}