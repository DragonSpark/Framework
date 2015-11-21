using DragonSpark.Activation;
using DragonSpark.Extensions;
using System;

namespace DragonSpark.Diagnostics
{
	public class ExceptionMessageContext
	{
		public ExceptionMessageContext( Exception exception, Guid? contextId )
		{
			Exception = exception;
			ContextId = contextId;
		}

		public Exception Exception { get; }

		public Guid? ContextId { get; }
	}

	public class ExceptionMessageFactory : FactoryBase<ExceptionMessageContext, string>
	{
		readonly IExceptionFormatter formatter;

		public ExceptionMessageFactory( IExceptionFormatter formatter )
		{
			this.formatter = formatter;
		}

		protected override string CreateFrom( Type resultType, ExceptionMessageContext parameter )
		{
			var result = formatter.FormatMessage( parameter.Exception, parameter.ContextId );
			return result;
		}
	}

	/*public static class ExtensionMethodSupport
	{
		public static void Factory<T>() where T : IFactory
		{
			var result = Services.Location.Locate<T>().Create( context );
		}
	}*/

	public static class DiagnosticExtensions
	{
		public static string GetMessage( this Exception exception, Guid? contextId = null )
		{
			var context = new ExceptionMessageContext( exception, contextId );
			var factory = Services.Location.Locate<ExceptionMessageFactory>();
			var result = factory.Create( context );
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
				// Log.Current.Error( exception );
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
		// public static ILogger Current => Services.Location.Locate<ILogger>() ?? NonLogger.Instance;

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

	class NonLogger : ILogger
	{
		public static NonLogger Instance { get; } = new NonLogger();

		public void Information( string message, Priority priority )
		{}

		public void Warning( string message, Priority priority )
		{}

		public void Exception( string message, Exception exception )
		{}

		public void Fatal( string message, Exception exception )
		{}
	}
}