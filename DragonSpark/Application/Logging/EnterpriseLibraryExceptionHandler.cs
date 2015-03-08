using System;
using DragonSpark.IoC;
using DragonSpark.Runtime;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using IExceptionHandler = DragonSpark.Runtime.IExceptionHandler;

namespace DragonSpark.Application.Logging
{
    [Singleton( typeof(IExceptionHandler) )]
    public class EnterpriseLibraryExceptionHandler : IExceptionHandler
    {
        readonly ExceptionManager manager;
        readonly string policyName;

        public EnterpriseLibraryExceptionHandler( ExceptionManager manager, string policyName = "Default Exception Policy" )
        {
            this.manager = manager;
            this.policyName = policyName;
        }

        public ExceptionHandlingResult Handle( Exception exception )
        {
            Exception resultingException;
            var handled = manager.HandleException( exception, policyName, out resultingException );
            var result = new ExceptionHandlingResult( !handled, resultingException ?? exception );
            return result;
        }
    }
}