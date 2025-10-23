using System;
using DragonSpark.Runtime;

namespace DragonSpark.Text;

public sealed class SnakeCase : IFormatter<ReadOnlyMemory<char>>
{
    public static SnakeCase Default { get; } = new();

    SnakeCase() {}

    public string Get(ReadOnlyMemory<char> parameter)
    {
        var span   = parameter.Span;
        var length = span.Length;

        // First pass: count underscores
        for (var i = 1; i < span.Length; i++)
        {
            if (char.IsUpper(span[i]))
            {
                length++;
            }
        }

        using var builder = ArrayBuilder.New<char>(length);
        for (var i = 0; i < span.Length; i++)
        {
            var c = span[i];
            if (i > 0 && char.IsUpper(c))
            {
                builder.UncheckedAdd('_');
            }
            builder.UncheckedAdd(char.ToLowerInvariant(c));
        }

        return new(builder.AsSpan());
    }
}