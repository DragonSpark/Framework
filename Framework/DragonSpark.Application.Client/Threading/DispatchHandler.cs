using System;
using System.Threading.Tasks;
using System.Windows.Threading;
using PostSharp.Patterns.Threading;

namespace DragonSpark.Application.Client.Threading
{
	public class DispatchHandler : IDispatchHandler
	{
		readonly Dispatcher dispatcher;

		public DispatchHandler( Dispatcher dispatcher )
		{
			this.dispatcher = dispatcher;
		}

		[Dispatched]
		public void Execute( Action target )
		{
			dispatcher.Invoke( target );
		}

		[Dispatched( true )]
		public void Start( Action target )
		{
			Execute( target );
		}

		public async void Delay( Action target, TimeSpan time )
		{
			await Task.Delay( time );
			Start( target );
		}
	}
}