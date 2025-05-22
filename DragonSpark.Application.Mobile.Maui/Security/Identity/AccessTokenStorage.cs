using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

public sealed class AccessTokenStorage : Variable<AccessTokenView>
{
    public static AccessTokenStorage Default { get; } = new();

    AccessTokenStorage() {}
}