using System.ServiceModel;

namespace DragonSpark.Server.Legacy.Configuration
{
	public class ServiceBasedLifetimeManager : IoCLifetimeManager<IoC.ServiceBasedLifetimeManager, ServiceHostBase>
	{}
}