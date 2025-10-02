using System;

namespace DragonSpark.Application.Mobile.Attestation;

public sealed record ExistingAttestationResult(Guid Identity, string KeyHash) : AttestationResult(KeyHash);