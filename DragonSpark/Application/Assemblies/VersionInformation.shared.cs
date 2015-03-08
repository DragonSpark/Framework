using System;
using System.Runtime.Serialization;

namespace DragonSpark.Application.Assemblies
{
	[DataContract( Namespace = "http://services.DragonSpark-framework.com/Application/Session" )]
	public class VersionInformation
	{
		public VersionInformation()
		{}

		public VersionInformation( Version version ) : this( version.Major, version.Minor, version.Revision, version.Build )
		{}

		public VersionInformation( int major, int minor, int revision, int build )
		{
			Major = major;
			Minor = minor;
			Revision = revision;
			Build = build;
		}

		[DataMember]
		public int Major { get; set; }

		[DataMember]
		public int Minor { get; set; }

		[DataMember]
		public int Revision { get; set; }

		[DataMember]
		public int Build { get; set; }

		public override string ToString()
		{
			var version = new Version( Major, Minor, Build, Revision );
			var result = version.ToString();
			return result;
		}
	}


}