using DragonSpark.Application.Client.Eventing;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms.ComponentModel
{
	/*public interface IScreenProfileProvider
	{
		Size GetCurrentSize();

		double GetScaleFactor();
	}

	class ScreenProfileProvider : IScreenProfileProvider
	{
		public Size GetCurrentSize()
		{
			throw new System.NotImplementedException();
		}

		public double GetScaleFactor()
		{
			throw new System.NotImplementedException();
		}
	}*/

	sealed class DeviceInfo : Xamarin.Forms.DeviceInfo
	{
		/*readonly IScreenProfileProvider provider;

		public DeviceInfo( IScreenProfileProvider provider )
		{
			this.provider = provider;
		}*/

		double scaleFactor = 1.0d;
		Size size;

		public DeviceInfo()
		{
			CurrentOrientation = DeviceOrientation.Landscape;
			this.Event<ShellScaleFactorChangedEvent>().Subscribe( d => scaleFactor = d );
			this.Event<ShellSizeChangedEvent>().Subscribe( d => size = d );
		}

		/*double scalingFactor;
		Size pixelScreenSize, scaledScreenSize;

		/*public DeviceInfo Initialized()
		{
			if ( !Assign() )
			{
				this.Event<ShellInitializedEvent>().Subscribe( OnReady );
			}
			return this;
		}

		void OnReady( Size size )
		{
			Assign();

			this.Event<ShellInitializedEvent>().Unsubscribe( OnReady );
		}#1#

		bool Assign()
		{
			var application = System.Windows.Application.Current;
			var content = application.MainWindow;
			scalingFactor = 1.0; // (double)content.ScaleFactor;
			pixelScreenSize = content.Transform( x => new Size( x.ActualWidth * scalingFactor, x.ActualHeight * scalingFactor ) );
			scaledScreenSize = content.Transform( x => new Size( x.ActualWidth,  x.ActualHeight ) );
			return content != null;
		}
*/
		public override Size PixelScreenSize
		{
			get { return size; }
		}

		public override Size ScaledScreenSize
		{
			get { return new Size( size.Width * ScalingFactor, size.Height * ScalingFactor ); }
		}

		public override double ScalingFactor
		{
			get { return scaleFactor; }
		}
	}
}