using DragonSpark.Contracts.Security;
using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Mobile.Attestation;

public interface IChallenge : IStopAware<ChallengeResponse>;