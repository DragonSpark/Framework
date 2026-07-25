using System.Text;
using System.Text.Json;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

sealed class DetermineResponse : IStopAware<DetermineContentsInput, string?>
{
    public static DetermineResponse Default { get; } = new();

    DetermineResponse()
        : this("application/json", StringComparison.OrdinalIgnoreCase, FrameworkSerializerOptions.Default) {}

    readonly string                _json;
    readonly StringComparison      _comparison;
    readonly JsonSerializerOptions _options;

    public DetermineResponse(string json, StringComparison comparison, JsonSerializerOptions options)
    {
        _json       = json;
        _comparison = comparison;
        _options    = options;
    }

    public async ValueTask<string?> Get(Stop<DetermineContentsInput> parameter)
    {
        var ((stream, contentType), stop) = parameter;
        var response = await new StreamReader(stream, Encoding.UTF8).ReadToEndAsync(stop).Off();
        if (!response.IsNullOrWhiteSpace() && contentType?.Contains(_json, _comparison) == true)
        {
            var token = JsonSerializer.Deserialize<AccessTokenResponse>(response, _options);
            if (!token?.AccessToken.IsNullOrEmpty() == true)
            {
                return response;
            }
        }

        return null;
    }
}