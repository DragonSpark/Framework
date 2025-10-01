using System;

namespace DragonSpark.Application.Mobile.Attestation;

public readonly record struct AttestationResult(Guid? Identity, string KeyHash, string Challenge, string Attestation);