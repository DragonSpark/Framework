using System;
using System.Runtime.Serialization;

namespace DragonSpark
{
	[Serializable]
	public class ClientException : Exception
	{
		readonly ClientExceptionDetail detail;
		readonly string ipAddress;
		readonly string machineName;

		public ClientException( ClientExceptionDetail detail, string ipAddress, string machineName ) : base( detail.Message )
		{
			this.detail = detail;
			this.ipAddress = ipAddress;
			this.machineName = machineName;
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
			get { return machineName; }
		}

		public override string Source
		{
			get { return ipAddress; }
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