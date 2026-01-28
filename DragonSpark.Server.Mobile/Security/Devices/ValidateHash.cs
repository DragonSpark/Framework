using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Server.Mobile.Security.Devices;

sealed class ValidateHash : ISelect<ValidateHashInput, AuthenticateResult?>
{
    public static ValidateHash Default { get; } = new();

    ValidateHash() : this(AuthenticateResult.Fail("Invalid DPoP signature")) {}

    readonly AuthenticateResult _result;

    public ValidateHash(AuthenticateResult result) => _result = result;

    public AuthenticateResult? Get(ValidateHashInput parameter)
    {
        var (record, signingInput, bytes) = parameter;
        using var ecdsa  = CreateEcdsa.Default.Get(new(record.X, record.Y));
        var       digest = ComposeDigest.Default.Get(signingInput);
        using var derSig = JoseToDer.Default.Get(bytes);
        return ecdsa.VerifyHash(digest.Span, derSig.Memory.Span) ? null : _result;
    }
}