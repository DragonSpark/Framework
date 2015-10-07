using DragonSpark.Activation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.Client.Threading
{
	public static class Dispatch
	{
		public static void Execute( Action target )
		{
			Services.With<IDispatchHandler>( x => x.Execute( target ) );
		}

		public static void Start( Action target )
		{
			Services.With<IDispatchHandler>( x => x.Start( target ) );
		}

		public static void Delay( Action target, TimeSpan time )
		{
			Services.With<IDispatchHandler>( x => x.Delay( target, time ) );
		}
	}

	public static class Background
	{
		static readonly SemaphoreSlim Locker = new SemaphoreSlim( 1, 1 );
		public static async void Locked( this Task @this )
		{
			await Locker.WaitAsync();
			await @this;
			Locker.Release();
		}
	}
}