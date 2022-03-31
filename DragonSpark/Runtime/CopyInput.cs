using System.IO;

namespace DragonSpark.Runtime;

public readonly record struct CopyInput(Stream Source, string Destination);