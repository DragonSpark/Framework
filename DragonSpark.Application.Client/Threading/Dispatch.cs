using DragonSpark.Activation;
using System;

namespace DragonSpark.Application.Client.Threading
{
	public static class Dispatch
	{
		public static void Execute( Action target )
		{
			ServiceLocation.With<IDispatchHandler>( x => x.Execute( target ) );
		}

		public static void Start( Action target )
		{
			ServiceLocation.With<IDispatchHandler>( x => x.Start( target ) );
		}

		public static void Delay( Action target, TimeSpan time )
		{
			ServiceLocation.With<IDispatchHandler>( x => x.Delay( target, time ) );
		}
	}
}