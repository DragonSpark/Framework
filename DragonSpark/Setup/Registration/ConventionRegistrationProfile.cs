using System;

namespace DragonSpark.Setup.Registration
{
	public class ConventionRegistrationProfile
	{
		public ConventionRegistrationProfile( Type[] candidates )
		{
			Candidates = candidates;
		}

		public Type[] Candidates { get; }
	}
}