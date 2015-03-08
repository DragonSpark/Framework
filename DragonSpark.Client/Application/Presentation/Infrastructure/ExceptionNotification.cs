using System;

namespace DragonSpark.Application.Presentation.Infrastructure
{
    public class ExceptionNotification : BasicNotification
	{
		public const string DefaultTitle = "An Exception Has Occured in This Application.";
		readonly Exception exception;
		readonly bool isServerException;
		readonly string details;

		public ExceptionNotification( Exception exception, string details, bool isServerException, string title = DefaultTitle ) : base( title )
		{
			this.exception = exception;
			this.isServerException = isServerException;
			this.details = details;
		}

		public bool IsServerException
		{
			get { return isServerException; }
		}

		public Exception Exception
		{
			get { return exception; }
		}

		public string Details
		{
			get { return details; }
		}
	}
}