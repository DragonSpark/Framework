namespace DragonSpark.Server.Mobile.Platforms.Android.Attestation.Records;

public sealed record AttestationOperations(INewAttestation New, IExistingAttestation Existing);