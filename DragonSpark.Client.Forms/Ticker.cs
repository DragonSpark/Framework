using System;
using System.Windows.Threading;

namespace DragonSpark.Application.Client.Forms
{
	class Ticker : global::Xamarin.Forms.Ticker
	{
		readonly DispatcherTimer timer;

		public Ticker()
		{
			timer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds( 15.0 )
			};
			timer.Tick += ( sender, args ) => SendSignals();
		}

		protected override void EnableTimer()
		{
			timer.Start();
		}

		protected override void DisableTimer()
		{
			timer.Stop();
		}
	}
}
