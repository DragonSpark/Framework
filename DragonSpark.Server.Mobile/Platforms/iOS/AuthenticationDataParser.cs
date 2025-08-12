using System;
using System.Buffers.Binary;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using PeterO.Cbor;

namespace DragonSpark.Server.Mobile.Platforms.iOS;

sealed class AuthenticationDataParser : ISelect<Array<byte>, AuthenticationData?>
{
    public static AuthenticationDataParser Default { get; } = new();

    AuthenticationDataParser() : this(55) {}

    readonly byte _length;

    public AuthenticationDataParser(byte length) => _length = length;

    public AuthenticationData? Get(Array<byte> parameter)
    {
        var                   authData   = parameter.Open();
        var                   fixedBytes = authData.Take(_length).ToArray();
        var                   handle     = GCHandle.Alloc(fixedBytes, GCHandleType.Pinned);
        AuthenticationDataRaw raw;
        try
        {
            raw = Marshal.PtrToStructure<AuthenticationDataRaw>(handle.AddrOfPinnedObject());
        }
        finally
        {
            handle.Free();
        }

        if (BitConverter.IsLittleEndian)
        {
            raw.Counter            = BinaryPrimitives.ReadUInt32BigEndian(authData.AsSpan(33, 4));
            raw.CredentialIdLength = BinaryPrimitives.ReadUInt16BigEndian(authData.AsSpan(53, 2));
        }

        if (_length + raw.CredentialIdLength <= authData.Length)
        {
            var credentialId = authData.Skip(_length).Take(raw.CredentialIdLength).ToArray();
            /*if (Convert.ToBase64String(credentialId) != clientKeyId)
                return (false, null, null, null);*/
            var bytes = authData.Skip(_length + raw.CredentialIdLength).ToArray();
            var key   = CBORObject.DecodeFromBytes(bytes);
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
                               new(raw.Aaguid,
                                   new(new(credentialId, raw.CredentialIdLength), new(publicKey, publicKeyHash))));
            }
        }

        return null;
    }
}