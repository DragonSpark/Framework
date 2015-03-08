using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DragonSpark.Application
{
	[DataContract]
	public partial class ClientExceptionDetail
	{
		public ClientExceptionDetail()
		{}

		public ClientExceptionDetail( Exception error )
		{
			Message = error.Message;
			StackTrace = error.StackTrace;
			ClientExceptionType = error.GetType().AssemblyQualifiedName;
			OnCreate();
		}

		partial void OnCreate();

		[DataMember]
		public string Message { get; set; }

		[DataMember]
		public string StackTrace { get; set; }

		[DataMember]
		public string ClientExceptionType { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Ip", Justification = "Resharper disagrees." ), DataMember]
		public IEnumerable<string> IpAddresses { get; set; }

		[DataMember]
		public string MachineName { get; set; }
	}
}