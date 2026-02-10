using System;
using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Server.Mobile.Security.Devices.Authentication;

sealed class ComposeJwsParserInput : IParser<JwsParserInput?>
{
    public static ComposeJwsParserInput Default { get; } = new();

    ComposeJwsParserInput() {}

    public JwsParserInput? Get(string parameter)
    {
        if (!parameter.IsNullOrWhiteSpace())
        {
            var span  = parameter.AsSpan();
            var first = span.IndexOf('.');
            if (first > 0)
            {
                var rest   = span[(first + 1)..];
                var second = rest.IndexOf('.');
                if (second > 0)
                {
                    return new(first, second);
                }
            }
        }

        return null;
    }
}