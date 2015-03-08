using System;
using System.Runtime.Serialization;
using DragonSpark.Application.Assemblies;

namespace DragonSpark.Application
{
	[DataContract( Namespace = "http://services.dragonspark.us/Application" )]
	public class ApplicationDetails
	{
		[DataMember]
		public string Title { get; set; }

		[DataMember]
		public string Product { get; set; }

		[DataMember]
		public string Company { get; set; }

		[DataMember]
		public Uri CompanyUri { get; set; }
		
		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public string Configuration { get; set; }

		[DataMember]
		public Uri SupportUri { get; set; }

		[DataMember]
		public Uri IssueTrackingUri { get; set; }

		[DataMember]
		public string SupportDescription { get; set; }

		[DataMember]
		public string Copyright { get; set; }

		[DataMember]
		public DateTime? OriginalLaunchDate { get; set; }

		[DataMember]
		public VersionInformation VersionInformation { get; set; }
	}
}