namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

public sealed record AttestationOperations(INewAttestation New, IExistingAttestation Existing);