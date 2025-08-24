using DragonSpark.Model.Sequences.Memory;
using DragonSpark.Text;
using System;
using System.Security.Cryptography;

namespace DragonSpark.Server.Security;

public sealed class Challenges : IText
{
    public static Challenges Default { get; } = new();

    Challenges() : this(NewLeasing<byte>.Default) {}

    readonly INewLeasing<byte> _leasing;
    readonly byte              _length;

    public Challenges(INewLeasing<byte> leasing, byte length = 32)
    {
        _leasing = leasing;
        _length  = length;
    }

    public string Get()
    {
        using var lease = _leasing.Get(_length);
        var       span  = lease.AsSpan();
        RandomNumberGenerator.Fill(span);
        var result = Convert.ToBase64String(span);
        return result;
    }
}