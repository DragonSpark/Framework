using System.Composition.Hosting;

namespace DragonSpark.Composition
{
	public sealed class ContainerServicesConfigurator : ContainerConfigurator
	{
		public static ContainerServicesConfigurator Default { get; } = new ContainerServicesConfigurator();
		ContainerServicesConfigurator() {}

		public override ContainerConfiguration Get( ContainerConfiguration parameter ) => parameter.WithProvider( ServicesExportDescriptorProvider.Default );
	}
}