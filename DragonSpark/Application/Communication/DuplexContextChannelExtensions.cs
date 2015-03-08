using System;
using System.ServiceModel;
using System.Threading;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Communication
{
	public static class DuplexContextChannelExtensions
	{
		static INotifyCallbackCompleted Resolve( IDuplexContextChannel channel )
		{
			switch ( channel.CallbackInstance.State )
			{
				case CommunicationState.Created:
					channel.CallbackInstance.Open();
					break;
			}
			var result = channel.CallbackInstance.GetServiceInstance() as INotifyCallbackCompleted;
			return result;
		}

		public static TResult WaitFor<TResult>( this IDuplexContextChannel target )
		{
			var result = default( TResult );
			Resolve( target ).NotNull( item =>
			                           	{
			                           		var reset = new ManualResetEvent( false );
			                           		EventHandler<CallbackResultEventArgs> handler = ( sender, args ) =>
			                           		                                                	{
			                           		                                                		result = (TResult)args.Result;
			                           		                                                		reset.Set();
			                           		                                                	};
			                           		item.Completed += handler;
			                           		reset.WaitOne();
			                           	} );
			return result;
		}
	}
}