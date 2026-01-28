using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class AuthenticateDevice : IAuthenticateDevice
{
    readonly IDeviceRegistry _devices;
    readonly DetermineTicket _ticket;

    public AuthenticateDevice(IDeviceRegistry devices, DetermineTicket ticket)
    {
        _devices = devices;
        _ticket  = ticket;
    }

    public async ValueTask<AuthenticateResult> Get(Stop<AuthenticateDeviceInput> parameter)
    {
        var ((subject, scheme), stop) = parameter;
        var header = subject.Request.Headers.Authorization.ToString();
        if (!header.IsNullOrWhiteSpace() && header.StartsWith("DevicePoP ", StringComparison.OrdinalIgnoreCase))
        {
            var deviceId = header["DevicePoP ".Length..].Trim();
            if (!deviceId.IsNullOrWhiteSpace())
            {
                var record = await _devices.Off(new(deviceId, stop));
                return record is { IsBlocked: false }
                           ? await _ticket.Off(new(new(subject, record, scheme), stop))
                           : AuthenticateResult.Fail("Unknown/blocked device");
            }

            return AuthenticateResult.Fail("Missing device id");
        }

        return AuthenticateResult.NoResult();
    }
}