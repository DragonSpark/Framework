using System;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

public sealed class ValidAttestationEnvironment : IValidAttestation
{
    public static ValidAttestationEnvironment Default { get; } = new();

    ValidAttestationEnvironment() : this(StringComparison.InvariantCultureIgnoreCase) {}

    readonly StringComparison _comparison;

    public ValidAttestationEnvironment(StringComparison comparison) => _comparison = comparison;

    public bool Get(ValidAttestationInput parameter)
    {
        var (attestation, _, _, _, environment, _) = parameter;
        var result = attestation.AuthenticationData.Credential.Environment.Equals(environment, _comparison);
        return result;
    }
}