using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using DragonSpark.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace DragonSpark.Application.Communication.Configuration
{
	static class ExceptionUtility
	{
		static readonly Regex GuidExpression =
			new Regex( "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}", RegexOptions.Compiled );

		/// <summary>
		/// Logs the server exception.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns></returns>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This method is design is to capture all exceptions." )]
		public static Guid LogServerException( Exception exception )
		{
			// try to get the handoling instance from the exception message or get a new one.
			var handlingInstanceId = GetHandlingInstanceId( exception );

			// Log exception info to configured log object.
			var logged = false;
			try
			{
				if ( Logger.IsLoggingEnabled() )
				{
					IDictionary<string, object> properties = new Dictionary<string, object>
					                                         	{ { "HandlingInstance ID:", handlingInstanceId } };
					Logger.Write( exception, properties );
					logged = true;
				}
			}
			catch ( Exception e )
			{
				// if we can't log, then trace the exception information
				Trace.TraceError( @"Unhandled error occurred while logging the original exception. Error ID: {0}
Logging Exception details:
{1}.", handlingInstanceId, e );
			}
			finally
			{
				if ( !logged )
				{
					// if we can't log, then trace the exception information
					Trace.TraceError( @"Unhandled error occurred while consuming this service. Error ID: {0}
Exception details:
{1}.", handlingInstanceId, exception );
				}
			}

			return handlingInstanceId;
		}

		/// <summary>
		/// Gets the handling instance id.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <returns></returns>
		public static Guid GetHandlingInstanceId( Exception exception )
		{
			return GetHandlingInstanceId( exception, Guid.NewGuid() );
		}

		/// <summary>
		/// Gets the handling instance id.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="optionalHandlingInstanceId">The optional handling instance id.</param>
		/// <returns></returns>
		public static Guid GetHandlingInstanceId( Exception exception, Guid optionalHandlingInstanceId )
		{
			var result = optionalHandlingInstanceId;

			var match = GuidExpression.Match( exception.Message );
			if ( match.Success )
			{
				result = new Guid( match.Value );
			}
			return result;
		}

		/// <summary>
		/// Formats the exception message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="handlingInstanceId">The handling instance id.</param>
		/// <returns></returns>
		public static string FormatExceptionMessage( string message, Guid handlingInstanceId )
		{
			if ( string.IsNullOrEmpty( message ) )
			{
				message = FormatExceptionMessage( Resources.Message_AnErrorHasOccuredWhileConsumingThisService, handlingInstanceId );
			}

			return Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionUtility.FormatExceptionMessage( message,
			                                                                                                        handlingInstanceId );
		}

		/// <summary>
		/// Gets the message from the exception.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="optionalMessage">The optional message.</param>
		/// <param name="handlingInstanceId">The handling instance id.</param>
		/// <returns></returns>
		public static string GetMessage( Exception exception, string optionalMessage, Guid handlingInstanceId )
		{
			var result = exception.Message;

			if ( !string.IsNullOrEmpty( optionalMessage ) )
			{
				result = FormatExceptionMessage( optionalMessage, handlingInstanceId );
			}

			return result;
		}
	}
}