using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Communication.Http.Security;

public interface ISaveTokenState : IStopAware<AccessTokenView>;