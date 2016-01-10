using DragonSpark.Activation.FactoryModel;
using DragonSpark.Diagnostics;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.Activation.IoC
{
	public class ServiceLocatorFactory : ActivateFactory<ServiceLocator>
	{
		readonly IMessageLogger logger;

		public ServiceLocatorFactory() : this( new RecordingMessageLogger() ) {}

		public ServiceLocatorFactory( [Required]IMessageLogger logger )
		{
			this.logger = logger;
		}

		protected override ServiceLocator Activate( ActivateParameter parameter ) => base.Activate( parameter ).With( locator =>
		{
			locator.Container.With( container =>
			{
				container.RegisterInstance( logger );
				container.Extend().Register( locator ).Extension<ObjectBuilderExtension>();
			} );
		} );
	}
}