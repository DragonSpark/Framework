using DragonSpark.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Xamarin.Forms;
using ImageSource = System.Windows.Media.ImageSource;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	public sealed class FileImageSourceHandler : IImageSourceHandler
	{
		public Task<ImageSource> LoadImageAsync( Xamarin.Forms.ImageSource imagesoure, CancellationToken cancelationToken = default( CancellationToken ) )
		{
			var source = imagesoure.AsTo<FileImageSource, ImageSource>( item => new BitmapImage( CreateUri( item ) ) );
			var result = Task.FromResult( source );
			return result;
		}

		static Uri CreateUri( FileImageSource item )
		{
			return new Uri( string.Format( "pack://siteoforigin:,,,/{0}", item.File ), UriKind.Absolute );
		}
	}
}
