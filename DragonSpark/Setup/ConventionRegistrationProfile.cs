using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Setup
{
	public class ConventionRegistrationProfile
	{
		readonly Assembly[] application;
		readonly Assembly[] include;
		readonly TypeInfo[] candidates;

		public ConventionRegistrationProfile( Assembly[] application, Assembly[] include, TypeInfo[] candidates )
		{
			this.application = application;
			this.include = include;
			this.candidates = candidates;
		}

		public ICollection<Assembly> Application
		{
			get { return application; }
		}

		public ICollection<Assembly> Include
		{
			get { return include; }
		}

		public ICollection<TypeInfo> Candidates
		{
			get { return candidates; }
		}
	}
}