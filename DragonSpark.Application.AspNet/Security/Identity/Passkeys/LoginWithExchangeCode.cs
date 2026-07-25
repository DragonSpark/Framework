using DragonSpark.Application.AspNet.Security.Identity.Bearer;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public sealed class LoginWithExchangeCode : IStopAware<string, IResult>
{
    readonly IDecryptToken          _decrypt;
    readonly ComposeAccessTokenView _view;
    readonly string                 _key;

    public LoginWithExchangeCode(IDecryptToken decrypt, ComposeAccessTokenView view)
        : this(decrypt, view, ResponseType.Default) {}

    public LoginWithExchangeCode(IDecryptToken decrypt, ComposeAccessTokenView view, string key)
    {
        _decrypt = decrypt;
        _view    = view;
        _key     = key;
    }

    public async ValueTask<IResult> Get(Stop<string> parameter)
    {
        var claims = await _decrypt.Off(parameter);
        var result =
            claims is not null
                ? claims.TryGetValue(_key, out var response) && response is string r
                      ? TypedResults.Ok(await _view.Off(r))
                      : Results.BadRequest(new { error = "missing_payload" })
                : Results.BadRequest(new { error = "invalid_code" });
        return result;
    }
}