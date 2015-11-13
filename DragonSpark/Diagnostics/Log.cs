using DragonSpark.Activation;
using DragonSpark.Extensions;
using System;

namespace DragonSpark.Diagnostics
{
	public static class DiagnosticExtensions
	{
		public static string GetMessage( this Exception exception, Guid? contextId = null )
		{
			var result = Services.Location.With<IExceptionFormatter, string>( y => y.FormatMessage( exception, contextId ) ) ?? exception.ToString();
			return result;
		}

		public static Exception Try( this Action action )
		{
			try
			{
				action();
			}
			catch ( Exception exception )
			{
				Log.Current.Error( exception );
				return exception;
			}
			return null;
		}

		public static void TryAndHandle( this Action action )
		{
			var exception = action.Try();
			exception.With( x => Services.Location.With<IExceptionHandler>( y => y.Process( x ) ) );
		}
	}

	public static class Log
	{
		public static ILogger Current => Services.Location.Locate<ILogger>();

		static void Handle( ILogger logger, Func<ILogger, Action<string, Exception>> getMethod, Exception exception, Guid? contextId = null )
		{
			var message = exception.GetMessage( contextId );
			var method = getMethod( logger );
			method( message, exception );
		}

		public static void Information( this ILogger @this, string message, Priority priority = Priority.Normal )
		{
			@this.Information( message, priority );
		}

		public static void Warning( this ILogger @this, string message, Priority priority = Priority.High )
		{
			@this.Warning( message, priority );
		}

		public static void Error( this ILogger @this, Exception exception, Guid? contextId = null )
		{
			Handle( @this, x => x.Exception, exception, contextId );
		}

		public static void Fatal( this ILogger @this, Exception exception, Guid? contextId = null )
		{
			Handle( @this, x => x.Fatal, exception, contextId );
		}
	}
}