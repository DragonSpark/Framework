using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Xamarin.Forms;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	public sealed class FileImageSourceHandler : IImageSourceHandler, IRegisterable
	{
		public Task<System.Windows.Media.ImageSource> LoadImageAsync(global::Xamarin.Forms.ImageSource imagesoure, CancellationToken cancelationToken = default(CancellationToken))
		{
			System.Windows.Media.ImageSource result = null;
			FileImageSource fileImageSource = imagesoure as FileImageSource;
			if (fileImageSource != null)
			{
				string file = fileImageSource.File;
				result = new BitmapImage(new Uri("/" + file, UriKind.Relative));
			}
			return Task.FromResult<System.Windows.Media.ImageSource>(result);
		}
	}
}
