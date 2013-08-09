using DragonSpark.IoC;
using DragonSpark.Runtime;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using IExceptionHandler = DragonSpark.Runtime.IExceptionHandler;

namespace DragonSpark.Logging
{
    [Singleton( typeof(IExceptionHandler) )]
    public class EnterpriseLibraryExceptionHandler : IExceptionHandler
    {
	    public const string DefaultExceptionPolicy = "Default Exception Policy";
	    readonly ExceptionManager manager;
        readonly string policyName;

        public EnterpriseLibraryExceptionHandler( ExceptionManager manager, string policyName = DefaultExceptionPolicy )
        {
            this.manager = manager;
            this.policyName = policyName;
        }

        public ExceptionHandlingResult Handle( Exception exception )
        {
            Exception resultingException;
            var rethrow = manager.HandleException( exception, policyName, out resultingException );
            var result = new ExceptionHandlingResult( rethrow, resultingException ?? exception );
            return result;
        }
    }

	public class LogExceptionHandler : Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.IExceptionHandler
	{
		public Exception HandleException( Exception exception, Guid handlingInstanceId )
		{
			Log.Error( exception, handlingInstanceId );
			return exception;
		}
	}

	public class LogFatalExceptionHandler : Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.IExceptionHandler
	{
		public Exception HandleException( Exception exception, Guid handlingInstanceId )
		{
			Log.Fatal( exception, handlingInstanceId );
			return exception;
		}
	}

	public class LogExceptionAsWarningHandler : Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.IExceptionHandler
	{
		public Exception HandleException( Exception exception, Guid handlingInstanceId )
		{
			Log.Warning( string.Format( "An exception of type '{0}' occured.  Message: '{1}'.  Context ID: '{2}'.", exception.GetType(), exception.Message, handlingInstanceId ) );
			return exception;
		}
	}

	public class LogExceptionAsInformationHandler : Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.IExceptionHandler
	{
		public Exception HandleException( Exception exception, Guid handlingInstanceId )
		{
			Log.Information( string.Format( "An exception of type '{0}' occured.  Message: '{1}'.  Context ID: '{2}'.", exception.GetType(), exception.Message, handlingInstanceId ) );
			return exception;
		}
	}
}