using DragonSpark.Application.Communication.Http.Security;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

sealed class AccessTokenStore : DragonSpark.Model.Operations.Results.Stop.ProcessStoring<AccessTokenView?>, IAccessTokenStore
{
    public AccessTokenStore(RefreshAwareAccessTokenStore refresh) : base(AccessTokenProcessValue.Default, refresh) {}
}