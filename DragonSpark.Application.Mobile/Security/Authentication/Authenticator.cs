using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Selection.Alterations;
using Windows.System;

namespace DragonSpark.Application.Mobile.Security.Authentication;

public class Authenticator : IAuthenticator
{
    readonly ISelecting<ValidateAddressInput, Uri?> _address;
    readonly IAlteration<Session>                   _compose;

    protected Authenticator(ISelecting<ValidateAddressInput, Uri?> address)
        : this(address, ComposeSession.Default) {}

    protected Authenticator(ISelecting<ValidateAddressInput, Uri?> address, IAlteration<Session> compose)
    {
        _address = address;
        _compose  = compose;
    }

    public async ValueTask<Uri?> Get(Token<Uri> parameter)
    {
        var (requestUri, token)   = parameter;
        var (identifier, address) = _compose.Get(new(requestUri));

        if (!await Launcher.LaunchUriAsync(address))
        {
            throw new InvalidOperationException("Could not launch oauth2 process");
        }

        return await _address.Await(new(identifier, token));
    }
}
