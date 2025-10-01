namespace DragonSpark.Server.Mobile;

public readonly record struct NewAttestationRecordInput(string KeyHash, string Challenge, string Attestation);