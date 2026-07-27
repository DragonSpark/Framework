using DragonSpark.Server.Mobile.Platforms.iOS.Attestation.Records;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Assertion;

public readonly record struct AssertionCounterInput(AssertionRequest Request, IAttestationRecord Attestation)
{
    public AssertionCounterInput(string Challenge, string Payload, IAttestationRecord Attestation)
        : this(new(Challenge, Convert.FromBase64String(Payload)), Attestation) {}
}