using System.IO;

namespace DragonSpark.Azure.Storage;

public readonly record struct AppendInput(string Name, string Type, Stream Stream);