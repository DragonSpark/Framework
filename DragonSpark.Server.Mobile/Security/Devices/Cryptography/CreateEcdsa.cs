using System.Security.Cryptography;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.WebUtilities;

namespace DragonSpark.Server.Mobile.Security.Devices.Cryptography;

sealed class CreateEcdsa : ISelect<CreateEcdsaInput, ECDsa>
{
    public static CreateEcdsa Default { get; } = new();

    CreateEcdsa() {}

    public ECDsa Get(CreateEcdsaInput parameter)
    {
        var (xUrl, yUrl) = parameter;
        var parameters = new ECParameters
        {
            Curve = ECCurve.NamedCurves.nistP256,
            Q     = new() { X = WebEncoders.Base64UrlDecode(xUrl), Y = WebEncoders.Base64UrlDecode(yUrl) }
        };
        return ECDsa.Create(parameters);
    }
}