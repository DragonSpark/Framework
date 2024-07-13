using DragonSpark.Model.Operations.Selection;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System.IO;
using System.Threading.Tasks;

namespace DragonSpark.Drawing;

public class LoadImageData : ISelecting<Image, byte[]>
{
	readonly IImageEncoder _encoder;

	public LoadImageData(IImageEncoder encoder) => _encoder = encoder;

	public async ValueTask<byte[]> Get(Image parameter)
	{
		using var stream = new MemoryStream();
		await parameter.SaveAsync(stream, _encoder);
		stream.Seek(0, SeekOrigin.Begin);
		return stream.ToArray();
	}
}