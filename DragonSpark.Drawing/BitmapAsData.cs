using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Drawing;

sealed class BitmapAsData : IArray<BitmapAsDataInput, byte>
{
	public static BitmapAsData Default { get; } = new();

	BitmapAsData() : this(MemoryStreams.Default) {}

	readonly IResult<MemoryStream> _streams;

	public BitmapAsData(IResult<MemoryStream> streams) => _streams = streams;

	public Array<byte> Get(BitmapAsDataInput parameter)
	{
		var (subject, format) = parameter;
		using var stream = _streams.Get();
		subject.Save(stream, format);
		return stream.ToArray();
	}
}