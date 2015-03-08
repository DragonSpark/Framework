using DragonSpark.Application.Presentation.ComponentModel;
using DragonSpark.Application.Presentation.Infrastructure;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Runtime;
using Microsoft.LightSwitch.Runtime.Shell.Implementation.Resources;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace DragonSpark.Application.Presentation
{
	[Singleton( typeof(IExceptionHandler), Priority = Priority.Lowest )]
	public class ExceptionHandler : ViewObject, IExceptionHandler
	{
		readonly IExceptionFormatter formatter;
		readonly IEnumerable<Type> serverExceptionTypes;

		public ExceptionHandler( IExceptionFormatter formatter, IEnumerable<Type> serverExceptionTypes )
		{
			this.formatter = formatter;
			this.serverExceptionTypes = serverExceptionTypes;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "This method is designed to capture all exceptions." )]
		public ExceptionHandlingResult Handle( Exception exception )
		{
			var handled = /*HandleWithService( exception ) ||*/ ResolveHandled( exception );
			var result = new ExceptionHandlingResult( handled, exception );
			return result;
		}

	    /*static bool HandleWithService( Exception e )
        {
            try
            {
                IErrorHandlerService errorHandler;
                var result = VsExportProviderService.TryGetExportedValue( Application.LocalScope, out errorHandler ) && errorHandler.TryHandle( new UnhandledExceptionException( e ), null );
                return result;
            }
            catch ( Exception )
            {
                return false;
            }
        }*/

		bool ResolveHandled( Exception exception )
		{
			try
			{
				var details = formatter.FormatMessage( exception );

				var isServerException = serverExceptionTypes.Any( x => x.IsInstanceOfType( exception ) );
				var notification = new ExceptionNotification( exception, details, isServerException );
				displayException.Raise( notification );

				return true;
			}
			catch ( Exception )
			{
                var errorMessage = exception.Transform( x => x.Message ) ?? string.Format( CultureInfo.CurrentCulture, Strings.App_SilverlightApplicationFailed, new object[0] );
                Action action = () => MessageBox.Show( errorMessage, string.Format( CultureInfo.CurrentCulture, Strings.Caption_Error, new object[0] ), 0 );
	            if ( Deployment.Current.Dispatcher.CheckAccess() )
	            {
	                action();
	            }
	            else
	            {
	                Deployment.Current.Dispatcher.BeginInvoke( action );
	            }
				return false;
			}
		}

		public IInteractionRequest DisplayException
		{
			get { return displayException; }
		}	readonly InteractionRequest<ExceptionNotification> displayException = new InteractionRequest<ExceptionNotification>();
	}
}