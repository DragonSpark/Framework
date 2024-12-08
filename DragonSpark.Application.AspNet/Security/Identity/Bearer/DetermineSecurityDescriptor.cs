using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

sealed class DetermineSecurityDescriptor : ExpirationAwareDescriptor<ClaimsIdentity>
{
	public DetermineSecurityDescriptor(IdentitySecurityDescriptor descriptor, BearerSettings settings)
		: base(descriptor, settings) {}
}