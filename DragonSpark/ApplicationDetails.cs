using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace DragonSpark
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

		/*[DataMember]
		public Uri IssueTrackingUri { get; set; }

		[DataMember]
		public Uri SupportUri { get; set; }

		[DataMember]
		public string SupportDescription { get; set; }*/

		[DataMember]
		public string Copyright { get; set; }

		[DataMember]
		public DateTime? DeploymentDate { get; set; }

		[DataMember, TypeConverter(typeof(VersionConverter))]
		public Version Version { get; set; }
	}
}