using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;

namespace DragonSpark.Activation.IoC
{
	public class ServiceLocatorFactory : ActivateFactory<ServiceLocator>
	{
		public ServiceLocatorFactory() : base( Activation.Activator.GetCurrent )
		{}

		protected override ServiceLocator Activate( ActivateParameter parameter ) 
			=> base.Activate( parameter ).WithSelf( locator => locator.Container.Extend().Register( locator ).Extension<ObjectBuilderExtension>() );
	}
}