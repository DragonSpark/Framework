using System;
using System.Buffers;
using System.Formats.Asn1;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Server.Mobile.Properties;
using DragonSpark.Text;
using Microsoft.Extensions.DependencyInjection;
using NetFabric.Hyperlinq;
using PeterO.Cbor;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

public sealed class Registrations : ICommand<IServiceCollection>
{
    public void Execute(IServiceCollection parameter) {}
}

sealed class RootCertificate : Instances<byte>
{
    public static RootCertificate Default { get; } = new();

    RootCertificate() : base(Resources.Apple_App_Attestation_Root_CA_pem) {}
}

public readonly record struct VerificationInput(string Challenge, string Input);

sealed class Verify : ICondition<VerificationInput>
{
    public static Verify Default { get; } = new();

    Verify() : this(AttestationParser.Default, ValidAttestation.Default) {}

    readonly IParser<Attestation?>             _parser;
    readonly ICondition<ValidAttestationInput> _valid;

    public Verify(IParser<Attestation?> parser, ICondition<ValidAttestationInput> valid)
    {
        _parser = parser;
        _valid  = valid;
    }

    public bool Get(VerificationInput parameter)
    {
        var (challenge, input) = parameter;
        var attestation = _parser.Get(input);
        if (attestation is not null)
        {
            /*var receipt = body["receipt"].GetByteString();
            // TODO Additional checks: app ID, environment, etc.
            // Store receipt/public key hash for future assertions*/

            var result = _valid.Get(new(attestation, challenge));
            return result;
        }

        return false;
    }
}

public readonly record struct ValidAttestationInput(Attestation Attestation, string Challenge);

sealed class ValidAttestation : ICondition<ValidAttestationInput>
{
    public static ValidAttestation Default { get; } = new();

    ValidAttestation() : this(EmbeddedHash.Default, PayloadHash.Default) {}

    readonly IArray<X509Certificate2, byte> _expected;
    readonly IArray<PayloadHashInput, byte> _actual;

    public ValidAttestation(IArray<X509Certificate2, byte> expected, IArray<PayloadHashInput, byte> actual)
    {
        _expected = expected;
        _actual   = actual;
    }

    public bool Get(ValidAttestationInput parameter)
    {
        var ((payload, certificate), challenge) = parameter;
        var expected = _expected.Get(certificate);
        var actual   = _actual.Get(new(payload, challenge));
        var result   = actual.Open().SequenceEqual(expected);
        return result;
    }
}

sealed class EmbeddedHash : IArray<X509Certificate2, byte>
{
    public static EmbeddedHash Default { get; } = new();

    EmbeddedHash() : this("1.2.840.113635.100.8.2", new(TagClass.ContextSpecific, 1)) {}

    readonly string  _oid;
    readonly Asn1Tag _tag;

    public EmbeddedHash(string oid, Asn1Tag tag)
    {
        _oid = oid;
        _tag = tag;
    }

    public Array<byte> Get(X509Certificate2 parameter)
    {
        var bytes    = parameter.Extensions.Single(e => e.Oid.Verify().Value == _oid).RawData;
        var sequence = new AsnReader(bytes, AsnEncodingRules.BER).ReadSequence();
        var result   = sequence.ReadOctetString(_tag);
        return result;
    }
}

public readonly record struct PayloadHashInput(Array<byte> Payload, string Challenge);

public sealed record Attestation(Array<byte> Payload, X509Certificate2 Certificate);

sealed class AttestationParser : IParser<Attestation?>
{
    readonly ArrayPool<byte[]> _pool;
    public static AttestationParser Default { get; } = new();

    AttestationParser() : this(RootCertificate.Default) {}

    readonly X509Certificate2 _root;

    public AttestationParser(Array<byte> root)
        : this(X509CertificateLoader.LoadCertificate(root), ArrayPool<byte[]>.Shared) {}

    public AttestationParser(X509Certificate2 root, ArrayPool<byte[]> pool)
    {
        _root = root;
        _pool = pool;
    }

    public Attestation? Get(string parameter)
    {
        var       actual       = Convert.FromBase64String(parameter);
        var       decode       = CBORObject.DecodeFromBytes(actual);
        var       body         = decode["attStmt"];
        using var pair         = body["x5c"].Values.AsValueEnumerable().Select(x => x.GetByteString()).ToArray(_pool);
        var       span         = pair.Memory.Span;
        var       certificate  = X509CertificateLoader.LoadCertificate(span[0]);
        var       intermediate = X509CertificateLoader.LoadCertificate(span[1]);
        var       chain        = new X509Chain();
        var       policy       = chain.ChainPolicy;
        var       store        = policy.ExtraStore;
        policy.RevocationMode = X509RevocationMode.NoCheck;
        store.Add(intermediate);
        store.Add(_root);

        var result = chain.Build(certificate) ? new Attestation(decode["authData"].GetByteString(), certificate) : null;
        return result;
    }
}

sealed class PayloadHash : IArray<PayloadHashInput, byte>
{
    public static PayloadHash Default { get; } = new();

    PayloadHash() : this(NewLeasing<byte>.Default) {}

    readonly INewLeasing<byte> _new;

    public PayloadHash(INewLeasing<byte> @new)
    {
        _new = @new;
    }

    public Array<byte> Get(PayloadHashInput parameter)
    {
        var (data, challenge) = parameter;

        var       hash    = SHA256.HashData(Convert.FromBase64String(challenge));
        using var leasing = _new.Get(data.Length + hash.Length.Grade());
        var       nonce   = leasing.Store;
        Buffer.BlockCopy(data, 0, nonce, 0, data.Length.Degrade());
        Buffer.BlockCopy(hash, 0, nonce, data.Length.Degrade(), hash.Length);
        var span   = leasing.AsSpan();
        var result = SHA256.HashData(span);
        return result;
    }
}