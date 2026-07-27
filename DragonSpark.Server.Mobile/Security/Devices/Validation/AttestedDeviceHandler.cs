using System.Security.Claims;
using DragonSpark.Application.AspNet.Security;
using DragonSpark.Compose;
using DragonSpark.Server.Mobile.Security.Devices.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DragonSpark.Server.Mobile.Security.Devices.Validation;

sealed class AttestedDeviceHandler : AuthorizationHandler<AttestedDeviceRequirement>
{
    readonly IIsAttested  _attested;
    readonly ICurrentStop _stop;
    readonly string       _claim;

    public AttestedDeviceHandler(IIsAttested attested, ICurrentStop stop)
        : this(attested, stop, DeviceClaimName.Default) {}

    public AttestedDeviceHandler(IIsAttested attested, ICurrentStop stop, string claim)
    {
        _attested   = attested;
        _stop       = stop;
        _claim = claim;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                         AttestedDeviceRequirement requirement)
    {
        var device = context.User.Verify().FindFirstValue(_claim);
        if (device is not null && await _attested.Off(new(device, _stop.Get())))
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail(new(this, "The device is not hardware-validated"));
        }
    }
}