using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Image = SixLabors.ImageSharp.Image;

namespace DragonSpark.Drawing;

public class LoadImage(Bitmap source, ImageFormat format) : IStopAware<Image>
{
	public ValueTask<Image> Get(CancellationToken parameter)
	{
		using var stream = new MemoryStream();
		source.Save(stream, format);
		stream.Seek(0, SeekOrigin.Begin);
		return Image.LoadAsync(stream, parameter).ToOperation();
	}
}