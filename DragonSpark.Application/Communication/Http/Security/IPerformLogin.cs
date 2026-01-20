using DragonSpark.Contracts.Security;
using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Communication.Http.Security;

public interface IPerformLogin : IPerformLogin<LoginRequest>;
public interface IPerformLogin<T> : IStopAware<T, AccessTokenResponse?> where T : Contracts.Security.LoginRequest;