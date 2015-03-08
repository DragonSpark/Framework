using System;
using DragonSpark.Extensions;

namespace DragonSpark.Runtime
{
	public static class Logging
	{
		public static void Information( string message, string category = "Information", Priority priority = Priority.Low )
		{
			ServiceLocation.With<ILogger>( x => x.Write( message, category, priority ) );
		}

		public static void Warning( string message, string category = "Warning", Priority priority = Priority.Normal )
		{
			ServiceLocation.With<ILogger>( x => x.Write( message, category, priority ) );
		}

		public static void Error( Exception exception, string category = "Error", Priority priority = Priority.Highest )
		{
			ServiceLocation.With<ILogger>( x =>
			{
				var message = ServiceLocation.Locate<IExceptionFormatter>().Transform( y => y.FormatMessage( exception ), exception.ToString );
				x.Write( message, category, priority );
			} );
		}
		
		public static void Trace( Action action, Guid? guid = null )
		{
			ServiceLocation.With<ITracer>( x => x.Trace( action, guid ?? Guid.NewGuid() ) );
		}

        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "By design." )]
        public static Exception Try( this Action action, Priority priority = Priority.Highest )
		{
			try
			{
				action();
			}
			catch ( Exception exception )
			{
				Error( exception, priority: priority );
				return exception;
			}
			return null;
		}

		public static void TryAndHandle( this Action action, Priority priority = Priority.Highest )
		{
			var exception = action.Try( priority );
			exception.NotNull( x => ServiceLocation.With<IExceptionHandler>( y => y.Handle( exception ).NotNull( z => z.Handled.IsFalse( () => { throw z.Exception; } ) ) ) );
		}
	}
}