using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Xamarin.Forms;

namespace DragonSpark.Application.Forms.Rendering
{
	public sealed class StreamImagesourceHandler : IImageSourceHandler, IRegisterable
	{
		public async Task<System.Windows.Media.ImageSource> LoadImageAsync(global::Xamarin.Forms.ImageSource imagesource, CancellationToken cancelationToken = default(CancellationToken))
		{
			BitmapImage bitmapImage = null;
			StreamImageSource streamImageSource = imagesource as StreamImageSource;
			if (streamImageSource != null && streamImageSource.Stream != null)
			{
				using (Stream stream = await streamImageSource.GetStreamAsync(cancelationToken))
				{
					bitmapImage = new BitmapImage { StreamSource = stream };
				}
			}
			return bitmapImage;
		}
	}
}
