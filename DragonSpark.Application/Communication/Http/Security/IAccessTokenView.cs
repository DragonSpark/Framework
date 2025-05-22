using DragonSpark.Model.Operations.Selection;

namespace DragonSpark.Application.Communication.Http.Security;

public interface IAccessTokenView : IStopAware<ChallengeRequest, AccessTokenResponse?>;