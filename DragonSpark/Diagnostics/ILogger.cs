using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using Activator = DragonSpark.Activation.Activator;

namespace DragonSpark.Diagnostics
{
	public interface ILogger
	{
		void Information( string message, Priority priority = Priority.Normal );

		void Warning( string message, Priority priority = Priority.High );

		void Exception( string message, Exception exception );

		void Fatal( string message, Exception exception );
	}

	public interface IRecordingLogger : ILogger
	{
		IEnumerable<Line> Lines { get; }
	}

	public class RecordingLogger : LoggerBase, IRecordingLogger
	{
		readonly IList<Line> lines = new List<Line>();

		public RecordingLogger() : this( ExceptionFormatter.Instance, CurrentTime.Instance )
		{}

		public RecordingLogger( IExceptionFormatter formatter, ICurrentTime time ) : base( formatter, time )
		{}

		public void Playback( Action<string> write )
		{
			lines.OrderBy( x => x.Time ).Apply( tuple => write( tuple.Message ) );
		}

		protected override void Write( Line line )
		{
			lines.Add( line );
		}

		public IEnumerable<Line> Lines => lines;
	}

	public class CompositeLogger : ILogger
	{
		readonly ILogger[] loggers;

		public CompositeLogger( params ILogger[] loggers )
		{
			this.loggers = loggers;
		}

		public void Information( string message, Priority priority )
		{
			loggers.Apply( logger => logger.Information( message, priority ) );
		}

		public void Warning( string message, Priority priority )
		{
			loggers.Apply( logger => logger.Warning( message, priority ) );
		}

		public void Exception( string message, Exception exception )
		{
			loggers.Apply( logger => logger.Exception( message, exception ) );
		}

		public void Fatal( string message, Exception exception )
		{
			loggers.Apply( logger => logger.Fatal( message, exception ) );
		}
	}

	/*public class ExceptionMessageContext
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
	}*/

	/*public static class ExtensionMethodSupport
	{
		public static void Factory<T>() where T : IFactory
		{
			var result = Services.Location.Locate<T>().Create( context );
		}
	}*/

	public class TryContext
	{
		readonly ILogger logger;

		public TryContext( ILogger logger )
		{
			this.logger = logger;
		}

		public Exception Try( Action action )
		{
			try
			{
				action();
			}
			catch ( Exception exception )
			{
				logger.Exception( "An exception has occurred while executing an application delegate.", exception );
				return exception;
			}
			return null;
		}
	}

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

	/*public static class Log
	{
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
		
	}*/

	/*class NonLogger : ILogger
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
	}*/
}