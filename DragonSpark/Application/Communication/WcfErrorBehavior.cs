using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Communication
{
	/// <summary>
	/// Allows an implementer to control the fault message returned to the caller and optionally perform custom error processing such as logging.
	/// </summary>
	public sealed class WcfErrorBehavior : IErrorHandler
	{
		void IErrorHandler.ProvideFault( Exception error, MessageVersion version, ref Message fault )
		{
			var exception = error.As<FaultException>();
			if ( exception == null )
			{
				// Add code here to build faultreason for client based on exception
				var faultReason = new FaultReason( error.Message );
				var exceptionDetail = new ExceptionDetail( error );

				// For security reasons you can also decide to not give the ExceptionDetail back to the client or change the message, etc
				var faultException = new FaultException<ExceptionDetail>( exceptionDetail, faultReason, FaultCode.CreateSenderFaultCode( new FaultCode( "0" ) ) );

				var messageFault = faultException.CreateMessageFault();
				fault = Message.CreateMessage( version, messageFault, faultException.Action );
			}
		}

		/// <summary>
		/// Handle all WCF Exceptions
		/// </summary>
		bool IErrorHandler.HandleError( Exception ex )
		{
			// return true means we handled the error.
			return true;
		}
	}
}
