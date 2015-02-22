using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Xamarin.Forms;

namespace DragonSpark.Application.Forms.Rendering
{
	public sealed class ImageLoaderSourceHandler : IImageSourceHandler, IRegisterable
	{
		public async Task<System.Windows.Media.ImageSource> LoadImageAsync(global::Xamarin.Forms.ImageSource imagesoure, CancellationToken cancelationToken = default(CancellationToken))
		{
			BitmapImage bitmapImage = null;
			UriImageSource uriImageSource = imagesoure as UriImageSource;
			if (uriImageSource != null && uriImageSource.Uri != null)
			{
				using (Stream stream = await uriImageSource.GetStreamAsync(cancelationToken))
				{
					if (stream != null && stream.CanRead)
					{
						bitmapImage = new BitmapImage { StreamSource = stream };
					}
				}
			}
			return bitmapImage;
		}
	}
}
