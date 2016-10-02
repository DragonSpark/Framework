using DragonSpark.Activation;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Application.Setup
{
	public interface IServiceRepository : IActivator, IRepository<object>
	{
		void Add( InstanceRegistrationRequest request );
	}

	public interface IServiceAware
	{
		Type ServiceType { get; }
	}
}