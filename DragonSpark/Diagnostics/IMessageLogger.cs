using DragonSpark.Activation.FactoryModel;
using DragonSpark.Properties;
using DragonSpark.Runtime;
using PostSharp.Patterns.Contracts;
using System;
using System.Globalization;

namespace DragonSpark.Diagnostics
{
	public interface IMessageLogger
	{
		void Log( Message message );
	}

	public static class MessageLoggerExtensions
	{
		public static void Information( this IMessageLogger @this, string message, Priority priority = Priority.Normal ) => new LogInformationCommand( @this ).Execute( new MessageParameter( message, priority ) );

		public static void Warning( this IMessageLogger @this, string message, Priority priority = Priority.High ) => new LogWarningCommand( @this ).Execute( new MessageParameter( message, priority ) );

		public static void Exception( this IMessageLogger @this, string message, Exception exception ) => new LogExceptionCommand( @this ).Execute( new ExceptionMessageParameter( message, exception ) );

		public static void Fatal( this IMessageLogger @this, string message, FatalApplicationException exception ) => new LogFatalExceptionCommand( @this ).Execute( new FatalExceptionMessageParameter( message, exception ) );
	}

	public class LogExceptionCommand : LogMessageCommand<ExceptionMessageParameter>
	{
		public static LogExceptionCommand Instance { get; } = new LogExceptionCommand();

		public LogExceptionCommand() : this( MessageLogger.Instance ) { }

		public LogExceptionCommand( IMessageLogger logger ) : this( ExceptionMessageFactory.Instance, logger ) { }

		public LogExceptionCommand( IFactory<ExceptionMessageParameter, Message> factory, IMessageLogger logger ) : base( factory.Create, logger.Log ) { }
	}

	public class LogFatalExceptionCommand : LogMessageCommand<FatalExceptionMessageParameter>
	{
		public static LogFatalExceptionCommand Instance { get; } = new LogFatalExceptionCommand();

		public LogFatalExceptionCommand() : this( MessageLogger.Instance ) { }

		public LogFatalExceptionCommand( IMessageLogger logger ) : this( FatalExceptionMessageFactory.Instance, logger ) { }

		public LogFatalExceptionCommand( IFactory<FatalExceptionMessageParameter, Message> factory, IMessageLogger logger ) : base( factory.Create, logger.Log ) { }
	}

	public class LogWarningCommand : LogMessageCommand<MessageParameter>
	{
		public static LogWarningCommand Instance { get; } = new LogWarningCommand();

		public LogWarningCommand() : this( MessageLogger.Instance ) { }

		public LogWarningCommand( IMessageLogger logger ) : this( WarningMessageFactory.Instance, logger ) { }

		public LogWarningCommand( IFactory<MessageParameter, Message> factory, IMessageLogger logger ) : base( factory.Create, logger.Log ) { }
	}

	public class LogInformationCommand : LogMessageCommand<MessageParameter>
	{
		public static LogInformationCommand Instance { get; } = new LogInformationCommand();

		public LogInformationCommand() : this( MessageLogger.Instance ) {}

		public LogInformationCommand( IMessageLogger logger ) : this( InformationMessageFactory.Instance, logger ) {}

		public LogInformationCommand( IFactory<MessageParameter, Message> factory, IMessageLogger logger ) : base( factory.Create, logger.Log ) {}
	}

	public abstract class LogMessageCommand<T> : Command<T> where T : MessageParameter
	{
		readonly Func<T, Message> factory;
		readonly Action<Message> logger;

		protected LogMessageCommand( [Required]Func<T, Message> factory, [Required]Action<Message> logger )
		{
			this.factory = factory;
			this.logger = logger;
		}

		protected override void OnExecute( T parameter )
		{
			var message = factory( parameter );
			logger( message );
		}
	}

	public class FatalExceptionMessageFactory : ExceptionMessageFactoryBase<FatalExceptionMessageParameter>
	{
		public static FatalExceptionMessageFactory Instance { get; } = new FatalExceptionMessageFactory();

		public const string Category = "Fatal";

		public FatalExceptionMessageFactory() : this( CurrentTime.Instance, ExceptionFormatter.Instance ) { }

		public FatalExceptionMessageFactory( ICurrentTime time, IExceptionFormatter formatter ) : base( time, formatter, Category ) { }
	}

	public class ExceptionMessageFactory : ExceptionMessageFactoryBase<ExceptionMessageParameter>
	{
		public static ExceptionMessageFactory Instance { get; } = new ExceptionMessageFactory();

		public const string Category = "Exception";

		public ExceptionMessageFactory() : this( CurrentTime.Instance, ExceptionFormatter.Instance ) {}

		public ExceptionMessageFactory( ICurrentTime time, IExceptionFormatter formatter ) : base( time, formatter, Category ) {}
	}

	public abstract class ExceptionMessageFactoryBase<T> : MessageFactoryBase<T> where T : ExceptionMessageParameter
	{
		protected ExceptionMessageFactoryBase( ICurrentTime time, IExceptionFormatter formatter, string category ) : this( () => time.Now, formatter.Format, category ) { }

		protected ExceptionMessageFactoryBase( Func<DateTimeOffset> time, Func<Exception, string> format, string category ) : base( time, p => $"{p.Message} - {format( p.Exception )}", category ) {}
	}

	public class WarningMessageFactory : MessageFactoryBase<MessageParameter>
	{
		public const string Category = "Warning";

		public static WarningMessageFactory Instance { get; } = new WarningMessageFactory();

		public WarningMessageFactory() : base( Category )
		{}

		public WarningMessageFactory( Func<DateTimeOffset> time ) : base( time, Category )
		{}
	}

	public class InformationMessageFactory : MessageFactoryBase<MessageParameter>
	{
		public const string Category = "Information";

		public static InformationMessageFactory Instance { get; } = new InformationMessageFactory();

		public InformationMessageFactory() : base( Category )
		{}

		public InformationMessageFactory( Func<DateTimeOffset> time ) : base( time, Category )
		{}
	}

	public class FatalExceptionMessageParameter : ExceptionMessageParameter
	{
		public FatalExceptionMessageParameter( string message, FatalApplicationException exception, Priority priority = Priority.Highest ) : base( message, exception, priority )
		{}
	}

	public class ExceptionMessageParameter : MessageParameter
	{
		public ExceptionMessageParameter( [Required]string message, [Required]Exception exception, Priority priority = Priority.High ) : base( message, priority )
		{
			Exception = exception;
		}

		public Exception Exception { get; }
	}

	public class MessageParameter
	{
		public MessageParameter( string message, Priority priority )
		{
			Message = message;
			Priority = priority;
		}

		public string Message { get; }
		public Priority Priority { get; }
	}

	public abstract class MessageFactoryBase<T> : FactoryBase<T, Message> where T : MessageParameter
	{
		readonly Func<DateTimeOffset> time;
		readonly Func<T, string> message;
		readonly string category;

		protected MessageFactoryBase( string category ) : this( () => CurrentTime.Instance.Now, category ) {}

		protected MessageFactoryBase( Func<DateTimeOffset> time, string category ) : this( time, p => p.Message, category ) {}

		protected MessageFactoryBase( [Required]Func<DateTimeOffset> time, [Required]Func<T, string> message, [Required]string category )
		{
			this.time = time;
			this.message = message;
			this.category = category;
		}

		protected override Message CreateItem( T parameter )
		{
			var current = time();
			var formatted = string.Format( CultureInfo.InvariantCulture, Resources.DefaultTextLoggerPattern, current, category, message( parameter ), parameter.Priority );
			var result = new Message( parameter.Priority, current, category, formatted );
			return result;
		}
	}
}