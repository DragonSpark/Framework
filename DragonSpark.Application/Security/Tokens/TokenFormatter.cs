using System;
using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Text;

namespace DragonSpark.Application.Security.Tokens;

public sealed class TokenFormatter : IFormatter<string>
{
    public static TokenFormatter Default { get; } = new();

    TokenFormatter() {}

    public string Get(string parameter)
    {
        var source = parameter.AsSpan().TrimEnd('=');
        var result = source.Length <= 256 ? stackalloc char[source.Length] : new char[source.Length];
        source.Replace(result, '+', '-');
        result.Replace('/', '_');
        return new(result);
    }
}

public sealed class MemoryTokenFormatter : ILease<ReadOnlyMemory<char>, char>
{
    public static MemoryTokenFormatter Default { get; } = new();

    MemoryTokenFormatter() : this(NewLeasing<char>.Default) {}

    readonly INewLeasing<char> _leasing;

    public MemoryTokenFormatter(INewLeasing<char> leasing) => _leasing = leasing;

    public Leasing<char> Get(ReadOnlyMemory<char> parameter)
    {
        var source = parameter.Span.TrimEnd('=');
        var result = _leasing.Get((uint)source.Length);
        var store = result.AsSpan();
        source.Replace(store, '+', '-');
        store.Replace('/', '_');
        return result;
    }
}