using System;
using System.Security.Cryptography;
using System.Text;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Security;

public class HashDataBase : IArray<string, byte>
{
    readonly Func<HashAlgorithm> _hash;
    readonly Encoding            _encoding;

    protected HashDataBase(Func<HashAlgorithm> hash, Encoding encoding)
    {
        _hash     = hash;
        _encoding = encoding;
    }

    public Array<byte> Get(string parameter)
    {
        using var context = _hash();
        return context.ComputeHash(_encoding.GetBytes(parameter));
    }
}