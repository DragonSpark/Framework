using System.Text.Json;
using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Identity.Bearer;
using DragonSpark.Compose;
using DragonSpark.Contracts.Security;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public sealed class LoginWithExchangeCode : IStopAware<string, IResult>
{
    readonly IDecryptToken         _decrypt;
    readonly JsonSerializerOptions _options;
    readonly string                _key;

    public LoginWithExchangeCode(IDecryptToken decrypt)
        : this(decrypt, FrameworkSerializerOptions.Default, ResponseType.Default) {}

    public LoginWithExchangeCode(IDecryptToken decrypt, JsonSerializerOptions options, string key)
    {
        _decrypt = decrypt;
        _options = options;
        _key     = key;
    }

    public async ValueTask<IResult> Get(Stop<string> parameter)
    {
        var claims = await _decrypt.Off(parameter);
        var result =
            claims is not null
                ? claims.TryGetValue(_key, out var response) && response is string r
                      ? TypedResults.Ok(JsonSerializer.Deserialize<AccessTokenResponse>(r, _options))
                      : Results.BadRequest(new { error = "missing_payload" })
                : Results.BadRequest(new { error = "invalid_code" });
        return result;
    }
}