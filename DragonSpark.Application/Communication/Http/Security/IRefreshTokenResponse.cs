using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Communication.Http.Security;

public interface IRefreshTokenResponse : IStopAware<AccessTokenResponse, AccessTokenResponse?>;