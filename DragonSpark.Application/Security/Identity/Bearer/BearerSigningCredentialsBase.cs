using DragonSpark.Model.Results;
using DragonSpark.Text;
using Microsoft.IdentityModel.Tokens;

namespace DragonSpark.Application.Security.Identity.Bearer;

public class BearerSigningCredentialsBase : Instance<SigningCredentials>
{
    protected BearerSigningCredentialsBase(BearerSettings settings, string algorithms)
        : this(new SymmetricSecurityKey(EncodedTextAsData.Default.Get(settings.Key)), algorithms) {}

    protected BearerSigningCredentialsBase(SecurityKey key, string algorithms) : base(new(key, algorithms)) {}
}