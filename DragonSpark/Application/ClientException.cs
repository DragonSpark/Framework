using System;
using System.Linq;
using System.Runtime.Serialization;

namespace DragonSpark.Application
{
	[Serializable]
	public class ClientException : Exception
	{
		readonly ClientExceptionDetail detail;

		public ClientException( ClientExceptionDetail detail ) : base( detail.Message )
		{
			this.detail = detail;
		}

		public ClientException( string message ) : base( message )
		{}

		public ClientException( string message, Exception innerException ) :
			base( message, innerException )
		{}

		protected ClientException( SerializationInfo info, StreamingContext context ) : base( info, context )
		{}

		public ClientException()
		{}

		public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			base.GetObjectData(info, context);
		}

		public string ClientMachineName
		{
			get { return detail.MachineName; }
		}

		public override string Source
		{
			get { return string.Join( "; ", detail.IpAddresses.Select( x => x.ToString() ) ); }
			set { base.Source = value; }
		}

		public string ExceptionType
		{
			get { return detail.ClientExceptionType; }
		}

		public override string StackTrace
		{
			get { return detail.StackTrace; }
		}
	}
}