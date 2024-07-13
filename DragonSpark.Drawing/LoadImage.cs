using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Image = SixLabors.ImageSharp.Image;

namespace DragonSpark.Drawing;

public class LoadImage(Bitmap source, ImageFormat format) : IResulting<Image>
{
	public ValueTask<Image> Get()
	{
		using var stream = new MemoryStream();
		source.Save(stream, format);
		stream.Seek(0, SeekOrigin.Begin);
		return Image.LoadAsync(stream).ToOperation();
	}
}