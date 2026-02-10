using DragonSpark.Model.Selection;
using DragonSpark.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

class TokenBase : IFormatter<ClaimsSecurityDescriptorInput>
{
    readonly ISelect<ClaimsSecurityDescriptorInput, SecurityTokenDescriptor> _descriptor;
    readonly JsonWebTokenHandler                                             _handler;

    protected TokenBase(ISelect<ClaimsSecurityDescriptorInput, SecurityTokenDescriptor> descriptor)
        : this(descriptor, WebTokenHandler.Default) {}

    protected TokenBase(ISelect<ClaimsSecurityDescriptorInput, SecurityTokenDescriptor> descriptor,
                        JsonWebTokenHandler handler)
    {
        _descriptor = descriptor;
        _handler    = handler;
    }

    public string Get(ClaimsSecurityDescriptorInput parameter)
    {
        var descriptor = _descriptor.Get(parameter);
        var result     = _handler.CreateToken(descriptor);
        return result;
    }
}