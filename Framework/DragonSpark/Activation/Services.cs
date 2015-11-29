using DragonSpark.Runtime;
using DragonSpark.Runtime.Values;

namespace DragonSpark.Activation
{
	public static class Services
	{
		public static IServiceLocation Location => AmbientValues.Get<IServiceLocation>() ?? ServiceLocation.Instance;
	}

	/*public interface IServiceLocationHost
	{
		IServiceLocation Location { get; }
	}

	public class ServiceLocationHost : IServiceLocationHost
	{
		public static ServiceLocationHost Instance { get; } = new ServiceLocationHost();

		public IServiceLocation Location => ServiceLocation.Instance;
	}*/


}