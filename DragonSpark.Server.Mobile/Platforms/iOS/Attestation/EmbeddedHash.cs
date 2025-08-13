using System.Formats.Asn1;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DragonSpark.Compose;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

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
        var bytes  = parameter.Extensions.Single(e => e.Oid.Verify().Value == _oid).RawData;
        var result = new AsnReader(bytes, AsnEncodingRules.BER).ReadSequence().ReadOctetString(_tag);
        return result;
    }
}