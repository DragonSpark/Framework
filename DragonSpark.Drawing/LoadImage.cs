using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System.Drawing;
using System.Drawing.Imaging;
using Image = SixLabors.ImageSharp.Image;

namespace DragonSpark.Drawing;

public sealed class LoadImage : ILoadImage
{
	public static LoadImage Default { get; } = new();

	LoadImage() : this(MemoryStreams.Default, "image/png") {}

	readonly IResult<Stream> _streams;
	readonly string          _png;

	public LoadImage(IResult<Stream> streams, string png)
	{
		_streams = streams;
		_png     = png;
	}

	public async ValueTask<Image> Get(Stop<LoadImageInput> parameter)
	{
		var ((stream, type), stop) = parameter;
		try
		{
			return await Image.LoadAsync(stream, stop).Off();
		}
		// ReSharper disable once UncatchableException
		catch (IndexOutOfRangeException) when (type == _png)
		{
			stream.Position = 0;

			using var       bitmap = new Bitmap(stream);
			await using var next   = _streams.Get();
			bitmap.Save(next, ImageFormat.Png);
			next.Position = 0;
			return await Image.LoadAsync(next, stop).Off();
		}
	}
}