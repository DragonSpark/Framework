using System.Security.Claims;
using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class Sign : Formatter<ClaimsIdentity>, ISign
{
    public Sign(ExceptionAwareIdentitySecurityDescriptor descriptor)
        : base(descriptor.Then().Select(IdentityTokenFormatter.Default)) {}
}