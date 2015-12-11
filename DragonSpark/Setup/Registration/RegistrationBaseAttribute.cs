using System;

namespace DragonSpark.Setup.Registration
{
	[AttributeUsage( AttributeTargets.Class )]
	public abstract class RegistrationBaseAttribute : Attribute
	{
		protected RegistrationBaseAttribute( IConventionRegistration registration )
		{
			Registration = registration;
		}

		public IConventionRegistration Registration { get; }
	}
}