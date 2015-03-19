using System;
using System.Collections.Generic;

namespace DragonSpark.Setup
{
	[AttributeUsage( AttributeTargets.Assembly )]
	public sealed class RegistrationAttribute : Attribute
	{
		readonly Priority priority;
		readonly Type[] ignoreForRegistration;

		public RegistrationAttribute( Priority priority, params Type[] ignoreForRegistration )
		{
			this.priority = priority;
			this.ignoreForRegistration = ignoreForRegistration;
		}

		public Priority Priority
		{
			get { return priority; }
		}

		public IEnumerable<Type> IgnoreForRegistration
		{
			get { return ignoreForRegistration; }
		}
	}
}