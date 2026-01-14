using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using Flurl;

namespace DragonSpark.Application.Mobile.Maui.Device.Security.Passkey;

public sealed class HostedLoginAddress : HostedAddressBase
{
    public HostedLoginAddress(PasskeyWorkflowSettings settings)
        : base(settings.Address.AppendPathSegment(settings.Login).ToUri(), PasskeyAccessTokenStore.Default) {}
}

// TODO
public sealed class PasskeyToken : Variable<AccessTokenView>
{
    public static PasskeyToken Default { get; } = new();

    PasskeyToken() {}
}

sealed class PasskeyAccessTokenStore : IAccessTokenStore
{
    public static PasskeyAccessTokenStore Default { get; } = new();

    PasskeyAccessTokenStore() : this(PasskeyToken.Default) {}

    readonly IMutable<AccessTokenView?> _token;

    public PasskeyAccessTokenStore(IMutable<AccessTokenView?> token) => _token = token;

    public ValueTask<AccessTokenView?> Get(CancellationToken parameter)
        => _token.TryPop(out var token) ? token.ToOperation() : default(AccessTokenView?).ToOperation();
}