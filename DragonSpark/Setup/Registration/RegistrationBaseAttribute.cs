using System;
using DragonSpark.Activation;

namespace DragonSpark.Setup.Registration
{
	[AttributeUsage( AttributeTargets.Class )]
	public abstract class RegistrationBaseAttribute : Attribute, IConventionRegistration
	{
		protected abstract void PerformRegistration( IServiceRegistry registry, Type subject );

		public void Register( IServiceRegistry registry, Type subject )
		{
			PerformRegistration( registry, subject );
		}
	}
}