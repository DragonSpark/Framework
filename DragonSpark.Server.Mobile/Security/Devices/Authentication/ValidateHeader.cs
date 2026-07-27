using System.Text.Json;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Authentication;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

sealed class ValidateHeader : ISelect<ReadOnlyMemory<byte>, AuthenticateResult?>
{
    public static ValidateHeader Default { get; } = new();

    ValidateHeader() : this(StringComparison.OrdinalIgnoreCase) {}

    readonly StringComparison _comparison;

    public ValidateHeader(StringComparison comparison) => _comparison = comparison;

    public AuthenticateResult? Get(ReadOnlyMemory<byte> parameter)
    {
        using var document = JsonDocument.Parse(parameter);
        var       header   = document.RootElement;
        return header.TryGetProperty("typ", out var typ) && string.Equals(typ.GetString(), "dpop+jwt", _comparison)
                   ? !header.TryGetProperty("alg", out var alg) || !string.Equals(alg.GetString(), "ES256", _comparison)
                         ? AuthenticateResult.Fail("Invalid alg")
                         : null
                   : AuthenticateResult.Fail("Invalid typ");
    }
}