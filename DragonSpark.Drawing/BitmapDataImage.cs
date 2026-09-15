using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using System.Drawing;
using System.Drawing.Imaging;
using Image = SixLabors.ImageSharp.Image;

namespace DragonSpark.Drawing;

public class BitmapDataImage : IStopAware<Image>
{
	readonly Array<byte>     _data;
	readonly IResult<Stream> _stream;

	protected BitmapDataImage(Bitmap source, ImageFormat format)
		: this(BitmapAsData.Default.Get(new(source, format)), MemoryStreams.Default) {}

	protected BitmapDataImage(Array<byte> data, IResult<Stream> stream)
	{
		_data   = data;
		_stream = stream;
	}

	public async ValueTask<Image> Get(CancellationToken parameter)
	{
		await using var stream = _stream.Get();
		stream.Write(_data.Open());
		stream.Seek(0, SeekOrigin.Begin);
		return await Image.LoadAsync(stream, parameter).Off();
	}
}