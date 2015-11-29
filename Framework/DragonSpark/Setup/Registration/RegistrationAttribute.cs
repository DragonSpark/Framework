using System;

namespace DragonSpark.Setup.Registration
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

		public Type[] IgnoreForRegistration { get; }
	}
}