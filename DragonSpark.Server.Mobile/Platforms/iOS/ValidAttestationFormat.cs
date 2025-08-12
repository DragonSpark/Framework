using System;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

sealed class ValidAttestationFormat : IValidAttestation
{
    public static ValidAttestationFormat Default { get; } = new();

    ValidAttestationFormat() {}

    public bool Get(ValidAttestationInput parameter)
    {
        var (attestation, _, _, _, format) = parameter;
        var result = attestation.Format.Equals(format, StringComparison.InvariantCultureIgnoreCase);
        return result;
    }
}