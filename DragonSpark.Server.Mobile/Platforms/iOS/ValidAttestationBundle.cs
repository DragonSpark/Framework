using System;
using DragonSpark.Model.Selection;
using DragonSpark.Text;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

sealed class ValidAttestationBundle : IValidAttestation
{
    public static ValidAttestationBundle Default { get; } = new();

    ValidAttestationBundle() : this(EncodedHashedText.Default) {}

    readonly ISelect<string, byte[]> _hash;

    public ValidAttestationBundle(ISelect<string, byte[]> hash) => _hash = hash;

    public bool Get(ValidAttestationInput parameter)
    {
        var (attestation, _, bundleId, _, _, _) = parameter;
        var result = attestation.AuthenticationData.RelyingPartyIdentifierHash.Open().SequenceEqual(_hash.Get(bundleId));
        return result;
    }
}