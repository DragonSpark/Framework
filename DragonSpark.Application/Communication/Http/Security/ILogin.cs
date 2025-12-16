using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Communication.Http.Security;

public interface ILogin : IStopAware<LoginRequest, AccessTokenResponse?>;