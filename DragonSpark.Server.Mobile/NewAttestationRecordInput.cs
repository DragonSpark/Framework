namespace DragonSpark.Server.Mobile;

public readonly record struct NewAttestationRecordInput(
    string Attestation,
    string KeyHash,
    string Challenge);