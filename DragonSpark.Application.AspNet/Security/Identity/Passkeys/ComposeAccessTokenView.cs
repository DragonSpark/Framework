using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Contracts.Security;
using DragonSpark.Model.Operations.Selection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Passkeys;

public sealed class ComposeAccessTokenView : ISelecting<string, AccessTokenView>
{
    readonly ICurrentContext       _context;
    readonly JsonSerializerOptions _options;

    public ComposeAccessTokenView(ICurrentContext context) : this(context, FrameworkSerializerOptions.Default) {}

    [Candidate(false)]
    public ComposeAccessTokenView(ICurrentContext context, JsonSerializerOptions options)
    {
        _context = context;
        _options = options;
    }

    public async ValueTask<AccessTokenView> Get(string parameter)
    {
        var response = JsonSerializer.Deserialize<AccessTokenResponse>(parameter, _options).Verify();

        var context = _context.Get();
        context.Request.Headers.Authorization = $"Bearer {response.AccessToken}";

        var result     = await context.AuthenticateAsync(IdentityConstants.BearerScheme).Off();
        var identifier = result.Principal.Verify().FindFirstValue(ClaimTypes.Email).Verify();
        return new(identifier, response);
    }
}