using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Activation
{
	public interface IServiceLocation
	{
		bool IsAvailable { get; }

		IServiceLocator Locator { get; }

		void Assign( IServiceLocator locator );
	}
}