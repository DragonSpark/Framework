using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;

namespace DragonSpark.Activation.IoC
{
	public class ServiceLocatorFactory : ActivateFactory<ServiceLocator>
	{
		public ServiceLocatorFactory() : base( SystemActivator.Instance )
		{}

		/*public ServiceLocatorFactory( IActivator activator ) : base( activator )
		{}*/

		protected override ServiceLocator Activate( ActivateParameter parameter )
		{
			var result = base.Activate( parameter ).WithSelf( locator => locator.Container.Extend().Register( locator ).Extension<ObjectBuilderExtension>() );
			return result;
		}
	}
}