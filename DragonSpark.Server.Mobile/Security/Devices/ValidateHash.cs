using System;
using System.Security.Cryptography;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Authentication;
using NetFabric.Hyperlinq;

namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class ValidateHash : ISelect<ValidateHashInput, AuthenticateResult?>
{
    public static ValidateHash Default { get; } = new();

    ValidateHash() : this(CreateEcdsa.Default, ComposeDigest.Default, JoseToDer.Default) {}

    readonly ISelect<CreateEcdsaInput, ECDsa>                    _session;
    readonly ISelect<ReadOnlyMemory<char>, ReadOnlyMemory<byte>> _digest;
    readonly ISelect<ReadOnlyMemory<byte>, Lease<byte>>          _signature;

    public ValidateHash(ISelect<CreateEcdsaInput, ECDsa> session,
                        ISelect<ReadOnlyMemory<char>, ReadOnlyMemory<byte>> digest,
                        ISelect<ReadOnlyMemory<byte>, Lease<byte>> signature)
    {
        _session   = session;
        _digest    = digest;
        _signature = signature;
    }

    public AuthenticateResult? Get(ValidateHashInput parameter)
    {
        var (record, signingInput, bytes) = parameter;
        using var ecdsa  = _session.Get(new(record.X, record.Y));
        var       digest = _digest.Get(signingInput);
        using var derSig = _signature.Get(bytes);
        var       valid  = ecdsa.VerifyHash(digest.Span, derSig.Memory.Span, DSASignatureFormat.Rfc3279DerSequence);
        return valid ? null : AuthenticateResult.Fail("Invalid DPoP signature");
    }
}