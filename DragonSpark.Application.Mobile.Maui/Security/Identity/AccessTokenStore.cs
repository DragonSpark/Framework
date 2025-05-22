using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

public sealed class AccessTokenStore : Storing<AccessTokenView?>
{
    public static AccessTokenStore Default { get; } = new();

    AccessTokenStore() : base(AccessTokenStorage.Default, AccessTokenViewStorage.Default) {}
}