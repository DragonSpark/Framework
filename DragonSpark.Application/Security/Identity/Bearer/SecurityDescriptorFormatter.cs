using DragonSpark.Compose;
using DragonSpark.Text;
using System.Collections.Generic;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class SecurityDescriptorFormatter : Formatter<IDictionary<string, object>>
{
	public SecurityDescriptorFormatter(ExpirationAwareSecurityDescriptor descriptor)
		: base(descriptor.Then().Select(IdentityTokenFormatter.Default)) {}
}