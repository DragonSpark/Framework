using System;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Compose;
using Flurl;

namespace DragonSpark.Application.Mobile.Maui.Device.Security.Passkey;

public class HostedAddressBase : DragonSpark.Model.Operations.Results.Stop.IStopAware<Uri>
{
    readonly Uri               _root;
    readonly IAccessTokenStore _token;

    protected HostedAddressBase(Uri root, IAccessTokenStore token)
    {
        _root  = root;
        _token = token;
    }

    public async ValueTask<Uri> Get(CancellationToken parameter)
    {
        var token  = await _token.Verifying(parameter).Off();
        var result = _root.AppendQueryParam("token", token.Response.AccessToken).ToUri();
        return result;
    }
}