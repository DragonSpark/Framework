using System;
using System.Buffers;
using System.Security.Cryptography;
using System.Text;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.Security;

public class HexHashBase : IAlteration<string>
{
    readonly IArray<string, byte> _data;

    protected HexHashBase(Func<HashAlgorithm> hash, Encoding encoding) : this(new HashData(hash, encoding)) {}

    protected HexHashBase(IArray<string, byte> data)
    {
        _data = data;
    }

    public string Get(string parameter)
    {
        var       hash   = _data.Get(parameter);
        using var parts  = hash.AsValueEnumerable().Select(x => x.ToString("x2")).ToArray(ArrayPool<string>.Shared);
        var       result = string.Join(string.Empty, parts);
        return result;
    }
}