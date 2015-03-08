using System;
using DragonSpark.IoC;
using DragonSpark.Runtime;

namespace DragonSpark.Application
{
	[Singleton( typeof(IExceptionFormatter), Priority = Priority.Lowest )]
	public class ExceptionFormatter : IExceptionFormatter
	{
		readonly ApplicationDetails details;
		readonly StackTracePolicy policy;

		public ExceptionFormatter( ApplicationDetails details, StackTracePolicy policy = StackTracePolicy.Always )
		{
			this.details = details;
			this.policy = policy;
		}

		protected virtual string CreateMessage( Exception exception )
		{
			var stackTrace = policy == StackTracePolicy.Always || policy == StackTracePolicy.OnlyWhenDebuggingOrRunningLocally && Runtime.IsRunningUnderDebugOrLocalhost() ? ResolveStackTrace( exception ) : "Not available.";

			var result = string.Format( "Exception occured in application {1} ({2}).{0}[Version: {3}]{0}{0}{4}{0}{5}{0}{6}",
										System.Environment.NewLine,
										details.Title,
										details.Product,
										details.VersionInformation,
										exception.Message,
										exception.GetType(),
										stackTrace );
			return result;
		}

		string IExceptionFormatter.FormatMessage( Exception exception )
		{
			return FormatMessage( exception );
		}

		protected virtual string FormatMessage( Exception exception )
		{
			var result = CreateMessage( exception );
			return result;
		}

		protected virtual string ResolveStackTrace( Exception exception )
		{
			var result = exception.StackTrace;

			// Account for nested exceptions
			var innerException = exception.InnerException;
			while ( innerException != null )
			{
				result += string.Format( "{2}{2}Caused by: {0}{2}{1}", innerException.Message, innerException.StackTrace, System.Environment.NewLine );
				innerException = innerException.InnerException;
			}

			return result;
		}
	}
}