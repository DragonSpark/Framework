using DragonSpark.Model.Results;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

sealed class WebTokenHandler : Instance<JsonWebTokenHandler>
{
    public static WebTokenHandler Default { get; } = new();

    WebTokenHandler() : base(new()) {}
}