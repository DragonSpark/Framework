using DragonSpark.Application;
using DragonSpark.Application.Setup;
using DragonSpark.Diagnostics;

namespace DragonSpark.Activation.Location
{
	public sealed class DefaultServices : CompositeActivator
	{
		public static DefaultServices Default { get; } = new DefaultServices();
		DefaultServices() : base( 
			new InstanceRepository( GlobalServiceProvider.Default, Activator.Default, Exports.Default, ApplicationParts.Default, ApplicationAssemblies.Default, ApplicationTypes.Default, LoggingHistory.Default, LoggingController.Default, Diagnostics.Defaults.Source, Instances.Default ), 
			new DecoratedActivator( Instances.Default.Get ),
			Activator.Default
		) {}
	}
}