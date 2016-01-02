using DragonSpark.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System;
using IExceptionHandler = DragonSpark.Diagnostics.IExceptionHandler;

namespace DragonSpark.Windows.Runtime
{
	public class ExceptionHandler : IExceptionHandler
	{
		public const string DefaultExceptionPolicy = "Default Exception Policy";
		readonly ExceptionManager manager;
		readonly string policyName;

		public ExceptionHandler( ExceptionManager manager, string policyName = DefaultExceptionPolicy )
		{
			this.manager = manager;
			this.policyName = policyName;
		}

		public virtual ExceptionHandlingResult Handle( Exception exception )
		{
			Exception resultingException;
			var rethrow = manager.HandleException( exception, policyName, out resultingException );
			var result = new ExceptionHandlingResult( rethrow, resultingException ?? exception );
			return result;
		}
	}
}