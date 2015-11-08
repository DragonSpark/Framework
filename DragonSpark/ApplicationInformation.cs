using System;

namespace DragonSpark
{
	public class ApplicationInformation
	{
		public string Title { get; set; }

		public string Product { get; set; }

		public string Company { get; set; }

		public Uri CompanyUri { get; set; }
		
		public string Description { get; set; }

		public string Configuration { get; set; }

		public string Copyright { get; set; }

		public DateTimeOffset? DeploymentDate { get; set; }

		public Version Version { get; set; }
	}
}