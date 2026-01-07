using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;
using Microsoft.Maui.ApplicationModel;

namespace DragonSpark.Application.Mobile.Maui.Device.Security.Passkey;

class LaunchHostedAddressBase : IStopAware
{
    readonly DragonSpark.Model.Operations.Results.Stop.IStopAware<Uri> _address;

    protected LaunchHostedAddressBase(DragonSpark.Model.Operations.Results.Stop.IStopAware<Uri> address)
        => _address = address;

    public async ValueTask Get(CancellationToken parameter)
    {
        await Browser.OpenAsync(await _address.Off(parameter)).Off();
    }
}