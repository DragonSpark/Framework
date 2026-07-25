using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Operations;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

sealed class DecryptToken : IDecryptToken
{
    readonly TokenValidationParameters _parameters;
    readonly JsonWebTokenHandler       _handler;

    public DecryptToken(SecureTokenValidation validation) : this(validation.Get(), WebTokenHandler.Default) {}

    [Candidate(false)]
    public DecryptToken(TokenValidationParameters parameters, JsonWebTokenHandler handler)
    {
        _parameters = parameters;
        _handler    = handler;
    }

    public async ValueTask<IDictionary<string, object>?> Get(Stop<string> parameter)
    {
        var validation = await _handler.ValidateTokenAsync(parameter, _parameters).Off();
        var result     = validation.IsValid ? validation.Claims : null;
        return result;
    }
}