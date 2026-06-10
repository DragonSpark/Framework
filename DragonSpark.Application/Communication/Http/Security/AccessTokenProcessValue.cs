using DragonSpark.Contracts.Security;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Communication.Http.Security;

public sealed class AccessTokenProcessValue : ProtectedVariable<AccessTokenView>
{
    public static AccessTokenProcessValue Default { get; } = new();

    AccessTokenProcessValue() {}
}