using System.IO;
using DragonSpark.Model.Selection;
using JetBrains.Annotations;

namespace DragonSpark.Runtime;

public sealed class CopyStream : ISelect<Stream, MemoryStream>
{
    public static CopyStream Default { get; } = new();

    CopyStream() { }

    [MustDisposeResource]
    public MemoryStream Get(Stream parameter)
    {
        var result = new MemoryStream();
        parameter.Seek(0, SeekOrigin.Begin);
        parameter.CopyTo(result);
        return result;
    }
}
