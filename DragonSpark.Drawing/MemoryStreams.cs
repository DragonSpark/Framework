using DragonSpark.Model.Results;
using Microsoft.IO;

namespace DragonSpark.Drawing;

public sealed class MemoryStreams : IResult<MemoryStream>
{
	public static MemoryStreams Default { get; } = new();

	MemoryStreams() : this(new()) {}

	readonly RecyclableMemoryStreamManager _streams;

	public MemoryStreams(RecyclableMemoryStreamManager streams) => _streams = streams;

	public MemoryStream Get() => _streams.GetStream();
}

