using DragonSpark.Activation;
using DragonSpark.Activation.Location;
using DragonSpark.Configuration;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Setup
{
	public sealed class ActivatorFactory : ConfigurationProvisionedSourceBase<IActivator>
	{
		public static ActivatorFactory Default { get; } = new ActivatorFactory();
		ActivatorFactory() : base( () => DefaultServices.Default ) {}

		public override IActivator Get() => base.Get().Cached();
	}
}