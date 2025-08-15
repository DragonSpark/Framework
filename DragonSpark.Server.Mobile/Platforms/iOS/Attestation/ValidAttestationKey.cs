using System;
using System.Security.Cryptography;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

public sealed class ValidAttestationKey : IValidAttestation
{
    public static ValidAttestationKey Default { get; } = new();

    ValidAttestationKey() {}

    public bool Get(AttestationInstanceInput parameter)
    {
        var (attestation, _, _, keyHash, _, _) = parameter;

        var key      = attestation.AuthenticationData.Credential.Credential.Identifier.Value;
        var expected = SHA256.HashData(key);
        var actual   = Convert.FromBase64String(keyHash);
        var result   = expected.SequenceEqual(actual);
        return result;
    }
}