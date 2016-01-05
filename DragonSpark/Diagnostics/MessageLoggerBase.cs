using System;
using System.Globalization;
using DragonSpark.Properties;
using DragonSpark.Runtime;

namespace DragonSpark.Diagnostics
{
	public abstract class MessageLoggerBase : IMessageLogger
	{
		protected MessageLoggerBase() : this( ExceptionFormatter.Instance, CurrentTime.Instance )
		{}

		protected MessageLoggerBase( IExceptionFormatter formatter, ICurrentTime time )
		{
			Formatter = formatter;
			Time = time;
		}

		protected IExceptionFormatter Formatter { get; }
		protected ICurrentTime Time { get; }

		public void Information( string message, Priority priority ) => Log( message, nameof(Information), priority );

		public void Warning( string message, Priority priority ) => Log( message, nameof(Warning), priority );

		public void Exception( string message, Exception exception ) => Log( FormatException( message, exception ), nameof(Exception), Priority.High );

		public void Fatal( string message, Exception exception ) => Log( FormatException( message, exception ), nameof(Fatal), Priority.Highest );

		protected virtual string FormatException( string message, Exception exception )
		{
			var result = $"{message} - {Formatter.Format( exception )}";
			return result;
		}

		void Log( string message, string category, Priority priority )
		{
			var time = Time.Now;
			var formatted = string.Format( CultureInfo.InvariantCulture, Resources.DefaultTextLoggerPattern, time, category, message, priority );
			var line = new Message( priority, time, category, formatted );
			Write( line );
		}

		protected abstract void Write( Message message );
	}
}