using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Java.Lang;
using Java.Security;
using Java.Security.Spec;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class SpkiPointDecoder : ISelect<byte[], ECPoint>
{
    public static SpkiPointDecoder Default { get; } = new();

    SpkiPointDecoder() : this(KeyFactory.GetInstance("EC").Verify(), Class.FromType(typeof(ECPublicKeySpec))) {}

    readonly KeyFactory _factory;
    readonly Class      _class;

    public SpkiPointDecoder(KeyFactory factory, Class @class)
    {
        _factory = factory;
        _class   = @class;
    }

    public ECPoint Get(byte[] spki)
    {
        var spec    = new X509EncodedKeySpec(spki);
        var @public = _factory.GeneratePublic(spec);
        var result = _factory.GetKeySpec(@public, _class) is ECPublicKeySpec key
                         ? key.GetW().Verify()
                         : throw new InvalidOperationException();
        return result;
    }
}