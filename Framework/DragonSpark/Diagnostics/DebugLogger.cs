using DragonSpark.Properties;
using DragonSpark.Runtime;
using System;
using System.Diagnostics;
using System.Globalization;

namespace DragonSpark.Diagnostics
{
	public class DebugLogger : LoggerBase
	{
		public static DebugLogger Instance { get; } = new DebugLogger();

		public DebugLogger() : this( ExceptionFormatter.Instance, CurrentTime.Instance )
		{}

		public DebugLogger( IExceptionFormatter formatter, ICurrentTime time ) : base( formatter, time )
		{}

		protected override void Write( Line line )
		{
			Debug.WriteLine( line.Message );
		}
	}

	public class Line
	{
		public Line( Priority priority, DateTimeOffset time, string category, string message )
		{
			Priority = priority;
			Time = time;
			Category = category;
			Message = message;
		}

		public Priority Priority { get; }

		public DateTimeOffset Time { get; }

		public string Category { get; }

		public string Message { get; }
	}

	
	public abstract class  LoggerBase : ILogger
	{
		protected LoggerBase() : this( ExceptionFormatter.Instance, CurrentTime.Instance )
		{}

		protected LoggerBase( IExceptionFormatter formatter, ICurrentTime time )
		{
			Formatter = formatter;
			Time = time;
		}

		protected IExceptionFormatter Formatter { get; }
		protected ICurrentTime Time { get; }

		public void Information( string message, Priority priority )
		{
			Log( message, nameof(Information), priority );
		}

		public void Warning( string message, Priority priority )
		{
			Log( message, nameof(Warning), priority );
		}

		public void Exception( string message, Exception exception )
		{
			Log( FormatException( message, exception ), nameof(Exception), Priority.High );
		}

		public void Fatal( string message, Exception exception )
		{
			Log( FormatException( message, exception ), nameof(Fatal), Priority.Highest );
		}

		protected virtual string FormatException( string message, Exception exception )
		{
			var result = $"{message} - {Formatter.FormatMessage( exception )}";
			return result;
		}

		void Log( string message, string category, Priority priority )
		{
			var time = Time.Now;
			var formatted = string.Format( CultureInfo.InvariantCulture, Resources.DefaultTextLoggerPattern, time, category.ToUpper(), message, priority );
			var line = new Line( priority, time, category, formatted );
			Write( line );
		}

		protected abstract void Write( Line line );
	}
}
