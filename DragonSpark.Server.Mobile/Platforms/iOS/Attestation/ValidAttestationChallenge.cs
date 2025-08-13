using System;
using System.Security.Cryptography.X509Certificates;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

public sealed class ValidAttestationChallenge : IValidAttestation
{
    public static ValidAttestationChallenge Default { get; } = new();

    ValidAttestationChallenge() : this(EmbeddedHash.Default, PayloadHash.Default) {}

    readonly IArray<X509Certificate2, byte> _expected;
    readonly IArray<PayloadHashInput, byte> _actual;

    public ValidAttestationChallenge(IArray<X509Certificate2, byte> expected, IArray<PayloadHashInput, byte> actual)
    {
        _expected = expected;
        _actual   = actual;
    }

    public bool Get(ValidAttestationInput parameter)
    {
        var (attestation, challenge, _, _, _, _) = parameter;
        var expected = _expected.Get(attestation.Statement.Certificate);
        var actual   = _actual.Get(new(attestation.AuthenticationData.Payload, challenge));
        var result   = actual.Open().SequenceEqual(expected);
        return result;
    }
}