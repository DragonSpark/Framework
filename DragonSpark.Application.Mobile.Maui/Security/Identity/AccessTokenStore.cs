using DragonSpark.Application.Communication.Http.Security;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

sealed class AccessTokenStore : DragonSpark.Model.Operations.Results.Stop.Storing<AccessTokenView?>, IAccessTokenStore
{
    // TODO: MutableAware
    public AccessTokenStore(RefreshAwareAccessTokenStore refresh) : base(AccessTokenStorage.Default, refresh) {}
}