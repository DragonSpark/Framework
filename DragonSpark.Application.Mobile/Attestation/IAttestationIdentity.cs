using DragonSpark.Model.Operations.Results.Stop;

namespace DragonSpark.Application.Mobile.Attestation;

public interface IAttestationIdentity : IStopAware<ExistingAttestationResult?>;