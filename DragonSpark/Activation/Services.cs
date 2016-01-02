using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Activation
{
	public static class Services
	{
		static Services()
		{
			Initialize( ServiceLocation.Instance );
		}

		public static void Initialize( IServiceLocation location )
		{
			Location = location;

			ServiceLocator.SetLocatorProvider( GetLocator );
		}

		static IServiceLocator GetLocator() => Location.Item;

		public static IServiceLocation Location { get; private set; }
	}
}