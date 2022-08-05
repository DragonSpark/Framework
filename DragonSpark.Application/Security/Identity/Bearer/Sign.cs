using DragonSpark.Compose;
using DragonSpark.Text;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class Sign : Formatter<ClaimsIdentity>, ISign
{
	public Sign(DetermineSecurityDescriptor descriptor) : base(descriptor.Then().Select(IdentityToken.Default)) {}
}