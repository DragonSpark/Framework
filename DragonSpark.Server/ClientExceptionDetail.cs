using System;
using System.Runtime.Serialization;

namespace DragonSpark
{
	[DataContract]
	public class ClientExceptionDetail
	{
		public ClientExceptionDetail()
		{}

		public ClientExceptionDetail( Exception error )
		{
			Message = error.Message;
			StackTrace = error.StackTrace;
			ClientExceptionType = error.GetType().AssemblyQualifiedName;
		}

		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public string StackTrace { get; set; }

		[DataMember]
		public string ClientExceptionType { get; set; }
	}
}
