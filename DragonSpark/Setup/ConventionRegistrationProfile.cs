using System;
using System.Reflection;

namespace DragonSpark.Setup
{
	public class ConventionRegistrationProfile
	{
		public ConventionRegistrationProfile( Assembly[] application, Assembly[] include, Type[] candidates )
		{
			Application = application;
			Include = include;
			Candidates = candidates;
		}

		public Assembly[] Application { get;  }

		public Assembly[] Include { get; }

		public Type[] Candidates { get; }
	}
}