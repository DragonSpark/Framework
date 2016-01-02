using System;

namespace DragonSpark.Setup.Registration
{
	[AttributeUsage( AttributeTargets.Assembly )]
	public sealed class RegistrationAttribute : PriorityAttribute
	{
		public RegistrationAttribute( params Type[] ignoreForRegistration ) : this( Priority.Normal, ignoreForRegistration )
		{}

		public RegistrationAttribute( Priority priority, params Type[] ignoreForRegistration ) : base( priority )
		{
			IgnoreForRegistration = ignoreForRegistration;
		}

		public string Namespaces { get; set; }

		public Type[] IgnoreForRegistration { get; }
	}
}