using DragonSpark.Activation;
using DragonSpark.Activation.Location;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Composition
{
	public sealed class ServiceProviderSource : SuppliedSource<IActivator, IActivator>
	{
		public static ServiceProviderSource Default { get; } = new ServiceProviderSource();
		ServiceProviderSource() : base( ServiceProviderFactory.Default.Get, DefaultServices.Default ) {}
	}
}