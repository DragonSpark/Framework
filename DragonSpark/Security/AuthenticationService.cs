using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;

namespace DragonSpark.Application.Communication.Security
{
    static class ExceptionSupport
    {
        // Methods
        static int GetExceptionErrorCode( Exception e )
        {
            if ( ( e is DataServiceOperationException ) || ( e is DataServiceException ) )
            {
                return 1;
            }
            if ( e is ValidationException )
            {
                return 4;
            }
            if ( e is ConcurrencyException )
            {
                return 3;
            }
            if ( e is PermissionException )
            {
                return 2;
            }
            return 0;
        }

        static int GetExceptionHttpStatusCode( Exception e )
        {
            if ( e is ConcurrencyException )
            {
                return 0x19c;
            }
            return e is DataServiceException ? ( (DataServiceException)e ).StatusCode : 500;
        }

        static string SerializeException( Exception e, Action<Exception, XmlWriter> serializeCustomExceptionInfo )
        {
            var exception = e as DataServiceOperationException;
            var output = new StringBuilder();
            using ( var writer = XmlWriter.Create( output ) )
            {
                string message;
                writer.WriteStartElement( "ExceptionInfo" );
                var flag = ShouldSerializeDebugInformation();
                if ( flag )
                {
                    Exception innerException;
                    if ( ( exception != null ) && exception.IsWrapper )
                    {
                        innerException = exception.InnerException;
                    }
                    else
                    {
                        innerException = e;
                    }
                    message = innerException.Message;
                    for ( innerException = innerException.InnerException; innerException != null; innerException = innerException.InnerException )
                    {
                        message = message + System.Environment.NewLine + "ServerGeneratedResources.ExceptionHelper_InnerExceptionMessage" + System.Environment.NewLine + innerException.Message;
                    }
                }
                else
                {
                    message = e.Message;
                }
                writer.WriteElementString( "Message", message );
                if ( serializeCustomExceptionInfo != null )
                {
                    serializeCustomExceptionInfo( e, writer );
                }
                if ( flag )
                {
                    string stackTrace;
                    if ( ( exception != null ) && exception.IsWrapper )
                    {
                        stackTrace = exception.InnerException.StackTrace;
                    }
                    else
                    {
                        stackTrace = e.StackTrace;
                    }
                    if ( !string.IsNullOrEmpty( stackTrace ) )
                    {
                        writer.WriteElementString( "StackTrace", stackTrace );
                    }
                }
                if ( exception != null )
                {
                    writer.WriteElementString( "ErrorInfo", exception.ErrorInfo );
                }
                writer.WriteEndElement();
            }
            return output.ToString();
        }

        static bool ShouldSerializeDebugInformation()
        {
            var current = HttpContext.Current;
            return ( ( current != null ) && !current.IsCustomErrorEnabled );
        }

        public static Exception TransformServiceException( Exception e )
        {
            while ( ( e is TargetInvocationException ) && ( e.InnerException != null ) )
            {
                e = e.InnerException;
            }
            if ( ( ( e is DataServiceOperationException ) || ( e is ValidationException ) ) || ( ( e is ConcurrencyException ) || ( e is PermissionException ) ) )
            {
                return e;
            }
            if ( e is UnauthorizedAccessException )
            {
                return new PermissionException( e.Message, e );
            }
            return new DataServiceOperationException( e.Message, null, e, null, true );
        }

        public static Exception TransformServiceExceptionForClient( Exception e )
        {
            if ( e is SqlException )
            {
                e = new DataServiceOperationException( "ServerGeneratedResources.ExceptionHelper_DatabaseError", null, e, null, false );
            }
            e = TransformServiceException( e );
            var message = SerializeException( e, null );
            return new DomainException( message, GetExceptionErrorCode( e ) );
        }

        public static Exception TransformServiceExceptionForODataClient( Exception e, Action<Exception, XmlWriter> serializeCustomExceptionInfo )
        {
            if ( e is SqlException )
            {
                e = new DataServiceOperationException( "ServerGeneratedResources.ExceptionHelper_DatabaseError", null, e, null, false );
            }
            if ( !( e is DataServiceException ) )
            {
                e = TransformServiceException( e );
            }
            var exceptionHttpStatusCode = GetExceptionHttpStatusCode( e );
            var exceptionErrorCode = GetExceptionErrorCode( e );
            return new DataServiceException( exceptionHttpStatusCode, exceptionErrorCode.ToString( CultureInfo.CurrentCulture ), SerializeException( e, serializeCustomExceptionInfo ), null, e );
        }
    }

    public class DataServiceOperationException : Exception
    {
        // Fields
        readonly string stackTrace;

        // Methods
        public DataServiceOperationException()
        {}

        public DataServiceOperationException( string message ) : base( message )
        {}

        public DataServiceOperationException( string message, Exception innerException ) : base( message, innerException )
        {}

        public DataServiceOperationException( string message, string errorInfo ) : this( message, errorInfo, null, null, false )
        {}

        internal DataServiceOperationException( string message, string errorInfo, Exception innerException, string stackTrace, bool isWrapper ) : base( message, innerException )
        {
            ErrorInfo = errorInfo;
            this.stackTrace = stackTrace;
            IsWrapper = isWrapper;
        }

        // Properties
        public string ErrorInfo { get; set; }

        internal bool IsWrapper { get; private set; }

        public override string StackTrace
        {
            get { return ( stackTrace ?? base.StackTrace ); }
        }
    }
}
