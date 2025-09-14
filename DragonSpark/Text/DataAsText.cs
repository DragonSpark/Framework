using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Text;

public sealed class DataAsText : Select<byte[], string>, IFormatter<byte[]>
{
	public static DataAsText Default { get; } = new();

	DataAsText() : base(Convert.ToBase64String) {}
}

// TODO

public sealed class MemoryAsText : Select<ReadOnlyMemory<byte>, string>, IFormatter<ReadOnlyMemory<byte>>
{
	public static MemoryAsText Default { get; } = new();

	MemoryAsText() : base(x => Convert.ToBase64String(x.Span)) {}
}