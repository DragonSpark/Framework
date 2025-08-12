using System;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

sealed class ValidAttestationFormat : IValidAttestation
{
    public static ValidAttestationFormat Default { get; } = new();

    ValidAttestationFormat() : this(StringComparison.InvariantCultureIgnoreCase) {}

    readonly StringComparison _comparison;

    public ValidAttestationFormat(StringComparison comparison) => _comparison = comparison;

    public bool Get(ValidAttestationInput parameter)
    {
        var (attestation, _, _, _, _, format) = parameter;
        var result = attestation.Format.Equals(format, _comparison);
        return result;
    }
}