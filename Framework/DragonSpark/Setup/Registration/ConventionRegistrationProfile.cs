using System;
using System.Reflection;

namespace DragonSpark.Setup.Registration
{
	public class ConventionRegistrationProfile
	{
		public ConventionRegistrationProfile( Assembly[] application, Type[] candidates )
		{
			Application = application;
			Candidates = candidates;
		}

		public Assembly[] Application { get;  }

		public Type[] Candidates { get; }
	}
}