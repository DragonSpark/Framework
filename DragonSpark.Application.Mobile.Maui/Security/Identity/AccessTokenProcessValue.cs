using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

public sealed class AccessTokenProcessValue : Variable<AccessTokenView>
{
    public static AccessTokenProcessValue Default { get; } = new();

    AccessTokenProcessValue() {}
}