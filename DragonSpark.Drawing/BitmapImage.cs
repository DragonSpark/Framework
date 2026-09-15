using DragonSpark.Model.Operations.Results.Stop;
using DragonSpark.Model.Sequences;
using System.Drawing;
using System.Drawing.Imaging;
using Image = SixLabors.ImageSharp.Image;

namespace DragonSpark.Drawing;

public class BitmapImage : IStopAware<Image>
{
	readonly Array<byte> _data;

	protected BitmapImage(Bitmap source, ImageFormat format) : this(BitmapAsData.Default.Get(new(source, format))) {}

	protected BitmapImage(Array<byte> data) => _data = data;

	public ValueTask<Image> Get(CancellationToken parameter) => new(Image.Load(_data.Open()));
}