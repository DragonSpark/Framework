using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;
using System.Drawing;
using System.Drawing.Imaging;
using Image = SixLabors.ImageSharp.Image;

namespace DragonSpark.Drawing;

public class LoadImage : IStopAware<Image>
{
	readonly Bitmap      _source;
	readonly ImageFormat _format;
	readonly object      _lock;

	protected LoadImage(Bitmap source, ImageFormat format) : this(source, format, new()) {}

	protected LoadImage(Bitmap source, ImageFormat format, object @lock)
	{
		_source = source;
		_format = format;
		_lock   = @lock;
	}

	public ValueTask<Image> Get(CancellationToken parameter)
	{
		using var stream = new MemoryStream();
		Copy().Save(stream, _format);
		stream.Seek(0, SeekOrigin.Begin);
		return Image.LoadAsync(stream, parameter).ToOperation();
	}

	Bitmap Copy()
	{
		lock (_lock)
		{
			return new(_source);
		}
	}
}