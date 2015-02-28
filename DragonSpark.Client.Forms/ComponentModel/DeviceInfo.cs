using DragonSpark.Application.Client.Eventing;
using DragonSpark.Extensions;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms.ComponentModel
{
	sealed class DeviceInfo : Xamarin.Forms.DeviceInfo
	{
		Size pixelScreenSize, scaledScreenSize;
		double scalingFactor;

		public DeviceInfo Initialized()
		{
			if ( !Assign() )
			{
				this.Event<ShellInitializedEvent>().Subscribe( this, OnReady );
			}
			return this;
		}

		void OnReady( System.Windows.Application sender )
		{
			Assign();

			this.Event<ShellInitializedEvent>().Unsubscribe( OnReady );
		}

		bool Assign()
		{
			var application = System.Windows.Application.Current;
			var content = application.MainWindow;
			scalingFactor = 1.0; // (double)content.ScaleFactor;
			pixelScreenSize = content.Transform( x => new Size( x.ActualWidth * scalingFactor, x.ActualHeight * scalingFactor ) );
			scaledScreenSize = content.Transform( x => new Size( x.ActualWidth,  x.ActualHeight ) );
			return content != null;
		}

		public override Size PixelScreenSize
		{
			get { return pixelScreenSize; }
		}

		public override Size ScaledScreenSize
		{
			get { return scaledScreenSize; }
		}

		public override double ScalingFactor
		{
			get { return scalingFactor; }
		}
	}
}