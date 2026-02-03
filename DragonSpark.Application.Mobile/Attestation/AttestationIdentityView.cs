using System;

namespace DragonSpark.Application.Mobile.Attestation;

public sealed record AttestationIdentityView(Guid Identity, string KeyHash);