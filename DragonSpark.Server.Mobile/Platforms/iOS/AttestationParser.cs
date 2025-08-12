using System;
using System.Buffers;
using System.Security.Cryptography.X509Certificates;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Text;
using NetFabric.Hyperlinq;
using PeterO.Cbor;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

public sealed class AttestationParser : IParser<Attestation?>
{
    readonly ArrayPool<byte[]> _pool;
    public static AttestationParser Default { get; } = new();

    AttestationParser() : this(RootCertificate.Default) {}

    readonly X509Certificate2                          _root;
    readonly ISelect<Array<byte>, AuthenticationData?> _data;

    public AttestationParser(Array<byte> root)
        : this(X509CertificateLoader.LoadCertificate(root), AuthenticationDataParser.Default,
               ArrayPool<byte[]>.Shared) {}

    public AttestationParser(X509Certificate2 root, ISelect<Array<byte>, AuthenticationData?> data,
                             ArrayPool<byte[]> pool)
    {
        _root = root;
        _data = data;
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

        var valid = chain.Build(certificate);
        if (valid)
        {
            var authentication = _data.Get(decode["authData"].GetByteString());
            if (authentication is not null)
            {
                return new(decode["fmt"].AsString(), new(certificate, body["receipt"].GetByteString()), authentication);
            }
        }

        return null;
    }
}