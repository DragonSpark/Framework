using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Text;

public sealed class Base64Encode : Alteration<string>
{
    public static Base64Encode Default { get; } = new();

    Base64Encode() : base(EncodedTextAsData.Default.Then().Subject.Select(Base64EncodeData.Default)) {}
}