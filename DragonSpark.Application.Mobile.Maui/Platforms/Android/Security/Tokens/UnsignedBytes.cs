using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class UnsignedBytes : IAlteration<Array<byte>>
{
    public static UnsignedBytes Default { get; } = new();

    UnsignedBytes() {}

    public Array<byte> Get(Array<byte> parameter)
    {
        var bytes = parameter.Open();
        return bytes.Length > 1 && bytes[0] == 0x00 ? bytes[1..] : bytes;
    }
}