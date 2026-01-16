using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Selection;
using Microsoft.Maui.ApplicationModel;

namespace DragonSpark.Application.Mobile.Maui.Device.Security.Passkey;

class LaunchHostedAddressBase : IStopAware<string>
{
    readonly ISelect<string, Uri>  _address;

    protected LaunchHostedAddressBase(ISelect<string, Uri> address) => _address = address;

    public async ValueTask Get(Stop<string> parameter)
    {
        var address = _address.Get(parameter);
        await Browser.OpenAsync(address).Off();
    }
}