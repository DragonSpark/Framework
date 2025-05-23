using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Communication.Http.Security;

public interface IRefreshTokenResponse : IStopAware<AccessTokenResponse, AccessTokenResponse?>;