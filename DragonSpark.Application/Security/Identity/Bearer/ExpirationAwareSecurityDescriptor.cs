using System.Collections.Generic;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class ExpirationAwareSecurityDescriptor : ExpirationAwareDescriptor<IDictionary<string, object>>
{
	public ExpirationAwareSecurityDescriptor(SecurityDescriptorByClaims previous, BearerSettings settings)
		: base(previous, settings) {}
}