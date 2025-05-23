using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Communication.Http.Security;

public interface IUpdateTokenState : IStopAware<AccessTokenView?>;