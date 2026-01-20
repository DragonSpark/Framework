using DragonSpark.Contracts.Security;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Communication.Http.Security;

public interface ICompleteLogin : IStopAware<AccessTokenView?>;