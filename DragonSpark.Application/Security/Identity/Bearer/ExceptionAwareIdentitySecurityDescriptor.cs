using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class ExceptionAwareIdentitySecurityDescriptor : ExpirationAwareDescriptor<ClaimsIdentity>
{
	public ExceptionAwareIdentitySecurityDescriptor(IdentitySecurityDescriptor descriptor, BearerSettings settings)
		: base(descriptor, settings) {}
}