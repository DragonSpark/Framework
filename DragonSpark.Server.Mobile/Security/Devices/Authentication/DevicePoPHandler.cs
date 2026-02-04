using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

sealed class DevicePoPHandler : AuthenticationHandler<DevicePoPOptions>
{
    readonly IAuthenticateDevice _handle;

    // ReSharper disable once TooManyDependencies
    public DevicePoPHandler(IOptionsMonitor<DevicePoPOptions> options, ILoggerFactory logger, UrlEncoder encoder,
                            IAuthenticateDevice handle)
        : base(options, logger, encoder)
        => _handle = handle;

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        => _handle.Allocate(new(new(Context, Scheme.Name), Context.RequestAborted));
}