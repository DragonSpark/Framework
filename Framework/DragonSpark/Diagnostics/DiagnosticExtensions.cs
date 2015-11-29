using System;
using DragonSpark.Activation;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Diagnostics
{
	public static class DiagnosticExtensions
	{
		/*public static string GetMessage( this Exception exception, Guid? contextId = null )
		{
			var context = new ExceptionMessageContext( exception, contextId );
			var factory = Services.Location.Locate<ExceptionMessageFactory>();
			var result = factory.Create( context );
			return result;
		}*/

		public static Exception Try( this Action action )
		{
			var result = Activator.Current.Activate<TryContext>().Try( action );
			return result;
		}

		/*public static void TryAndHandle( this Action action )
		{
			var exception = action.Try();
			exception.With( x => Services.Location.With<IExceptionHandler>( y => y.Process( x ) ) );
		}*/
	}
}