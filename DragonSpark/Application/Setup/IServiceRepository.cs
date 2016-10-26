using DragonSpark.Activation;
using DragonSpark.Runtime;

namespace DragonSpark.Application.Setup
{
	public interface IServiceRepository : IActivator, IRepository<object>
	{
		void Add( ServiceRegistration request );
	}

	/*public interface IServiceAware
	{
		Type ServiceType { get; }
	}*/
}