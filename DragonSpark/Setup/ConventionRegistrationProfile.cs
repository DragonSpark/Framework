using System.Collections.Generic;
using System.Reflection;
using Dynamitey.DynamicObjects;

namespace DragonSpark.Setup
{
	public class ConventionRegistrationProfile
	{
		public ConventionRegistrationProfile( ICollection<Assembly> application, ICollection<Assembly> include, ICollection<TypeInfo> candidates )
		{
			Application = application;
			Include = include;
			Candidates = candidates;
		}

		public ICollection<Assembly> Application { get;  }

		public ICollection<Assembly> Include { get; }

		public ICollection<TypeInfo> Candidates { get; }
	}
}