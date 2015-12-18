using System;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Activation.IoC
{
	class ServiceLocationMonitor : IDisposable
	{
		readonly IServiceLocation location;
		readonly IServiceLocator locator;

		/*public ServiceLocationMonitor( IServiceLocator locator ) : this( Services.Location, locator )
		{}*/

		public ServiceLocationMonitor( IServiceLocation location, IServiceLocator locator )
		{
			this.location = location;
			this.locator = locator;
		}

		public void Dispose()
		{
			if ( location.IsAvailable && location.Locator == locator )
			{
				location.Assign( null );
			}
		}
	}
}