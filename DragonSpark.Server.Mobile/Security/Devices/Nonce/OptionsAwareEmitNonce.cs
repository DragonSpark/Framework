using System.Threading.Tasks;
using DragonSpark.Model.Operations;
using DragonSpark.Server.Mobile.Security.Devices.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DragonSpark.Server.Mobile.Security.Devices.Nonce;

sealed class OptionsAwareEmitNonce : Model.Operations.Stop.IStopAware<HttpContext>
{
    readonly IOptions<DevicePoPOptions> _options;
    readonly EmitNonce<DeviceNonce>     _previous;

    public OptionsAwareEmitNonce(IOptions<DevicePoPOptions> options, EmitNonce<DeviceNonce> previous)
    {
        _options  = options;
        _previous = previous;
    }

    public ValueTask Get(Stop<HttpContext> parameter)
        => _options.Value.RequireNonce ? _previous.Get(parameter) : ValueTask.CompletedTask;
}