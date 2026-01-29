using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using DragonSpark.Text;
using Java.Math;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class FormatPoint : IFormatter<BigInteger>
{
    public static FormatPoint Default { get; } = new();

    FormatPoint() : this(UnsignedBytes.Default, TokenDataFormatter.Default) {}

    readonly IAlteration<Array<byte>> _unsigned;
    readonly IFormatter<Array<byte>>  _formatter;

    public FormatPoint(IAlteration<Array<byte>> unsigned, IFormatter<Array<byte>> formatter)
    {
        _unsigned  = unsigned;
        _formatter = formatter;
    }

    public string Get(BigInteger parameter)
    {
        var input    = parameter.ToByteArray().Verify();
        var unsigned = _unsigned.Get(input);
        var result   = _formatter.Get(unsigned);
        return result;
    }
}