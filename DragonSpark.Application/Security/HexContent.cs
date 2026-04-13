using System.Text;
using DragonSpark.Model.Sequences;
using DragonSpark.Text;

namespace DragonSpark.Application.Security;

public sealed class HexContent : IFormatter<Array<byte>>
{
    public static HexContent Default { get; } = new();

    HexContent() {}

    public string Get(Array<byte> parameter)
    {
        var sb = new StringBuilder((int)(parameter.Length * 2));

        foreach (var b in parameter)
        {
            sb.Append($"{b:x2}");
        }

        return sb.ToString().ToUpperInvariant();
    }
}