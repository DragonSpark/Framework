namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

public readonly record struct AttestationRecordInput(string KeyHash, string Challenge, string Attestation);