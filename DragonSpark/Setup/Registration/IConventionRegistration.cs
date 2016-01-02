using System;
using DragonSpark.Activation;

namespace DragonSpark.Setup.Registration
{
	public interface IConventionRegistration
	{
		void Register( IServiceRegistry registry, Type subject );
	}
}