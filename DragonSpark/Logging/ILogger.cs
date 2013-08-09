using DragonSpark.IoC;
using DragonSpark.Runtime;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.IO;

namespace DragonSpark.Logging
{
	public interface ILogger
	{
		[Event( 1, Level = EventLevel.Informational )]
		void Information( string message, Priority priority );

		[Event( 2, Level = EventLevel.Warning )]
		void Warning( string message, Priority priority );

		[Event( 3, Level = EventLevel.Error )]
		void Error( string message, Exception error );

		[Event( 4, Level = EventLevel.Critical )]
		void Fatal( string message, Exception error );

		[Event( 5, Level = EventLevel.Informational )]
		void StartTrace( string associatedMessage, Guid associatedId );
		
		[Event( 6, Level = EventLevel.Informational )]
		void EndTrace( string associatedMessage, Guid associatedId, TimeSpan totalTime );
	}

	public interface ITracer
	{
		void Trace( Action action, string message, Guid? id  = null );
	}

	[Singleton( typeof(ITracer), Priority = Priority.Lowest )]
	public class Tracer : ITracer
	{
		readonly ILogger logger;

		public Tracer( ILogger logger )
		{
			this.logger = logger;
		}

		public void Trace( Action action, string message, Guid? id = null )
		{
			var associatedId = id ?? Guid.NewGuid();
			var start = Stopwatch.GetTimestamp();
			logger.StartTrace( message, associatedId );
			action();
			var time = TimeSpan.FromTicks( Stopwatch.GetTimestamp() - start );
			logger.EndTrace( message, associatedId, time );
		}
	}

	[Singleton( typeof(IExceptionFormatter), Priority = Priority.Lowest )]
	public class ExceptionFormatter : IExceptionFormatter
	{
		readonly ApplicationDetails details;

		public ExceptionFormatter( ApplicationDetails details )
		{
			this.details = details;
		}

		protected virtual string CreateMessage( Exception exception, Guid? contextId )
		{
			var writer = new StringWriter();
			new TextExceptionFormatter( writer, exception, contextId.GetValueOrDefault() ).Format();
			var message = writer.GetStringBuilder();
			var result = string.Format( "Exception occured in application {1} ({2}).{0}[Version: {3}]{0}{0}{4}{0}{5}{0}Details:{0}=============================================={0}{6}",
			                            System.Environment.NewLine,
			                            details.Title,
			                            details.Product,
			                            details.Version,
			                            exception.Message,
			                            exception.GetType(),
			                            message );
			return result;
		}

		string IExceptionFormatter.FormatMessage( Exception exception, Guid? contextId )
		{
			return FormatMessage( exception, contextId );
		}

		protected virtual string FormatMessage( Exception exception, Guid? contextId )
		{
			var result = CreateMessage( exception, contextId );
			return result;
		}
	}
}