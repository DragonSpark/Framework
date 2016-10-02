using DragonSpark.ComponentModel;
using System;

namespace DragonSpark.Application
{
	public class ApplicationInformation
	{
		[Service]
		public AssemblyInformation AssemblyInformation { get; set; }

		public Uri CompanyUri { get; set; }
		
		public DateTimeOffset? DeploymentDate { get; set; }
	}
}