using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Logging
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
			ServiceLocation.With<ILogger>( x =>
			{
				var message = ServiceLocation.With<IExceptionFormatter,string>( y => y.FormatMessage( exception, contextId ) ) ?? exception.ToString();
				x.Error( message, exception );
			} );
		}

		public static void Fatal( Exception exception, Guid? contextId = null )
		{
			ServiceLocation.With<ILogger>( x =>
			{
				var message = ServiceLocation.With<IExceptionFormatter,string>( y => y.FormatMessage( exception, contextId ) ) ?? exception.ToString();
				x.Fatal( message, exception );
			} );
		}
		
		public static void Trace( string message, Action action, Guid? guid = null )
		{
			ServiceLocation.With<ITracer>( x => x.Trace( action, message, guid ) );
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "By design." )]
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
			exception.NotNull( x => ServiceLocation.With<IExceptionHandler>( y => y.Handle( exception ).With( z => z.RethrowRecommended.IsTrue( () => { throw z.Exception; } ) ) ) );
		}
	}
}