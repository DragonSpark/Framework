using System;
using DragonSpark.Activation;

namespace DragonSpark.Setup.Registration
{
	public interface IRegistration
	{
		void Register( IServiceRegistry registry, Type subject );
	}
}