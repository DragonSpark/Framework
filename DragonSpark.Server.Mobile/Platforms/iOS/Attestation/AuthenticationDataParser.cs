using System;
using System.Buffers.Binary;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Objects;
using PeterO.Cbor;

namespace DragonSpark.Server.Mobile.Platforms.iOS.Attestation;

sealed class AuthenticationDataParser : ISelect<Array<byte>, AuthenticationData?>
{
    public static AuthenticationDataParser Default { get; } = new();

    AuthenticationDataParser() : this(55, Encoding.UTF8, Materialize<AuthenticationDataRaw>.Default) {}

    readonly byte                                _length;
    readonly Encoding                            _encoding;
    readonly IMaterialize<AuthenticationDataRaw> _raw;

    public AuthenticationDataParser(byte length, Encoding encoding, IMaterialize<AuthenticationDataRaw> raw)
    {
        _length   = length;
        _encoding = encoding;
        _raw      = raw;
    }

    public AuthenticationData? Get(Array<byte> parameter)
    {
        var authData = parameter.Open();
        var data     = authData.Take(_length).ToArray();
        var raw      = _raw.Get(data);
        if (BitConverter.IsLittleEndian)
        {
            raw.Counter            = BinaryPrimitives.ReadUInt32BigEndian(authData.AsSpan(33, 4));
            raw.CredentialIdLength = BinaryPrimitives.ReadUInt16BigEndian(authData.AsSpan(53, 2));
        }

        if (_length + raw.CredentialIdLength <= authData.Length)
        {
            var credentialId = authData.Skip(_length).Take(raw.CredentialIdLength).ToArray();
            var bytes        = authData.Skip(_length + raw.CredentialIdLength).ToArray();
            var key          = CBORObject.DecodeFromBytes(bytes);
            switch (key[CBORObject.FromObject(1)].AsInt32())
            {
                case 2:
                    var x         = key[CBORObject.FromObject(-2)].GetByteString();
                    var y         = key[CBORObject.FromObject(-3)].GetByteString();
                    var publicKey = new byte[65];
                    publicKey[0] = 0x04;
                    Buffer.BlockCopy(x, 0, publicKey, 1, 32);
                    Buffer.BlockCopy(y, 0, publicKey, 33, 32);
                    var publicKeyHash = SHA256.HashData(publicKey);
                    return new(parameter, raw.RpIdHash, raw.Flags, raw.Counter,
                               new(_encoding.GetString(raw.Aaguid).Trim('\0'),
                                   new(new(credentialId, raw.CredentialIdLength), new(publicKey, publicKeyHash))));
            }
        }

        return null;
    }
}