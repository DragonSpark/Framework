using DragonSpark.Activation;
using DragonSpark.Activation.Location;
using DragonSpark.Extensions;
using DragonSpark.Sources.Scopes;

namespace DragonSpark.Application.Setup
{
	public sealed class ActivatorFactory : Scope<IActivator>
	{
		public static ActivatorFactory Default { get; } = new ActivatorFactory();
		ActivatorFactory() : base( () => DefaultServices.Default ) {}

		public override IActivator Get() => base.Get().Cached();
	}
}