using System;
using System.Collections.Generic;

namespace DragonSpark.Setup
{
	[AttributeUsage( AttributeTargets.Assembly )]
	public sealed class RegistrationAttribute : Attribute
	{
		public RegistrationAttribute( Priority priority, params Type[] ignoreForRegistration )
		{
			Priority = priority;
			IgnoreForRegistration = ignoreForRegistration;
		}

		public Priority Priority { get; }

		public string Namespaces { get; set; }

		public IEnumerable<Type> IgnoreForRegistration { get; }
	}
}