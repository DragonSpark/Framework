using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Security.Tokens;

public interface IDeviceKeyProvider : IStopAware<PublicJWK>;