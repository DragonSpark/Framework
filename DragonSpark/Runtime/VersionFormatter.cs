using System;
using DragonSpark.Text;

namespace DragonSpark.Runtime;

public sealed class VersionFormatter : IFormatter<Version>
{
    public static VersionFormatter Default { get; } = new();

    VersionFormatter() {}
    
    public string Get(Version parameter) => $"{parameter}.0";
}
