using System.ServiceModel;

namespace DragonSpark.Server.Legacy.Configuration
{
	public class UnityOperationContextLifetimeManager : IoCLifetimeManager<IoC.UnityOperationContextLifetimeManager, OperationContext>
	{}
}