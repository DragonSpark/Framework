using DragonSpark.Activation;
using DragonSpark.Application.Setup;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Composition
{
	public sealed class ServiceProviderFactory : AlterationBase<IActivator>
	{
		public static ServiceProviderFactory Default { get; } = new ServiceProviderFactory();
		ServiceProviderFactory() {}

		public override IActivator Get( IActivator parameter )
		{
			var context = CompositionHostFactory.Default.Get();
			var primary = new ServiceLocator( context );
			var result = new CompositeActivator( new InstanceRepository( context, primary ), primary, parameter );
			return result;
		}
	}
}
