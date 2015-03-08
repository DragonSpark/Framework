
using System;
using System.Windows.Threading;
using DragonSpark.IoC;

namespace DragonSpark.Application.Presentation
{
	[Singleton( typeof(IDelegateWorker), Name = "DispatcherDelegateWorker", Priority = Priority.Low )]
	public partial class DispatcherDelegateWorker : IDelegateWorker
	{
		readonly Dispatcher dispatcher;

		public DispatcherDelegateWorker( Dispatcher dispatcher )
		{
			this.dispatcher = dispatcher;
		}

		public IDelegateContext Execute( Action target )
		{
			if ( dispatcher.CheckAccess() )
			{
				target();
			}
			else
			{
				Start( target );
			}
			return null;
		}

		public IDelegateContext Start( Action target )
		{
			dispatcher.BeginInvoke( target );
			return null;
		}

		public IDelegateContext Delay( Action target, TimeSpan time )
		{
			var timer = CreateTimer( target, time );
			timer.Start();
			return null;
		}
	}
}