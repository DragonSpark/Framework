using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DragonSpark.Application.Client.Controls
{
	public static partial class WriteableBitmapExtensions
	{
		public static System.Windows.Media.Imaging.BitmapSource Rendered( this FrameworkElement @this, Size? size = null, Transform transform = null )
		{
			var dimensions = size.GetValueOrDefault( new Size( (int)@this.ActualWidth, (int)@this.ActualHeight ) );
			var source = PresentationSource.FromVisual( @this );
			var result = new RenderTargetBitmap( (int)dimensions.Width, (int)dimensions.Height, source.CompositionTarget.TransformToDevice.M11, source.CompositionTarget.TransformToDevice.M22, PixelFormats.Default );
			var visual = new DrawingVisual();
			using ( var context = visual.RenderOpen() )
			{
				var brush = new VisualBrush( @this );
				context.DrawRectangle( brush, null, new Rect( new Point(), new Size( @this.ActualWidth, @this.ActualHeight ) ) );
			}

			visual.Transform = transform ?? visual.Transform;

			result.Render( @this );
			result.Freeze();
			return result;
		}
		/*public static void BlitBlt(this WriteableBitmap dst, Point pt, WriteableBitmap src, Rect rc)
		{
			// crop rectangle
			if (rc.X + rc.Width > src.PixelWidth)
				rc.Width = src.PixelWidth - rc.X;
			if (rc.Y + rc.Height > src.PixelHeight)
				rc.Height = src.PixelHeight - rc.Y;
			if (pt.X + rc.Width > dst.PixelWidth)
				rc.Width = dst.PixelWidth - pt.X;
			if (pt.Y + rc.Height > dst.PixelHeight)
				rc.Height = dst.PixelHeight - pt.Y;

			// copy rectangle
			int[] srcPixels = src.Pixels;
			int[] dstPixels = dst.Pixels;
			int srcOffset = (int)(src.PixelWidth * rc.Y + rc.X) * 4;
			int dstOffset = (int)(dst.PixelWidth * pt.Y + pt.X) * 4;
			int max = (int)rc.Height;
			int len = (int)rc.Width * 4;

			for (int y = 0; y < max; y++)
			{
				Buffer.BlockCopy(srcPixels, srcOffset, dstPixels, dstOffset, len);
				srcOffset += src.PixelWidth * 4;
				dstOffset += dst.PixelWidth * 4;
			}
		}*/
	}
}