using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Model.Claims
{
	sealed class ClaimRegistration : Text.Text, IClaimRegistration
	{
		readonly IExternalClaim _registration;

		public ClaimRegistration(string instance, IExternalClaim registration) : base(instance)
			=> _registration = registration;

		public Claim Get(ExternalLoginInfo parameter) => _registration.Get(parameter);
	}
}