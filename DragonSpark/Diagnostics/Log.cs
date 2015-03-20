using DragonSpark.Activation;
using DragonSpark.Extensions;
using System;

namespace DragonSpark.Diagnostics
{
	public static class Log
	{
		public static void Information( string message, Priority priority = Priority.Normal )
		{
			ServiceLocation.With<ILogger>( x => x.Information( message, priority ) );
		}

		public static void Warning( string message, Priority priority = Priority.High )
		{
			ServiceLocation.With<ILogger>( x => x.Warning( message, priority ) );
		}

		public static void Error( Exception exception, Guid? contextId = null )
		{
			Handle( x => x.Exception, exception, contextId );
		}

		public static void Fatal( Exception exception, Guid? contextId = null )
		{
			Handle( x => x.Fatal, exception, contextId );
		}

		public static string GetMessage( this Exception exception, Guid? contextId = null )
		{
			var result = ServiceLocation.With<IExceptionFormatter, string>( y => y.FormatMessage( exception, contextId ) ) ?? exception.ToString();
			return result;
		}

		/*public static void Trace( this Action action, string message, Guid? guid = null )
		{
			ServiceLocation.With<ITracer>( x => x.Trace( action, message, guid ) );
		}*/

		public static Exception Try( this Action action )
		{
			try
			{
				action();
			}
			catch ( Exception exception )
			{
				Error( exception );
				return exception;
			}
			return null;
		}

		public static void TryAndHandle( this Action action )
		{
			var exception = action.Try();
			exception.NotNull( x => ServiceLocation.With<IExceptionHandler>( y => y.Process( x ) ) );
		}

		static void Handle( Func<ILogger, Action<string, Exception>> determineHandler, Exception exception, Guid? contextId = null )
		{
			ServiceLocation.With<ILogger>( x =>
			{
				var message = exception.GetMessage( contextId );
				var handler = determineHandler( x );
				handler( message, exception );
			} );
		}
	}
}