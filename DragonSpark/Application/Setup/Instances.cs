using DragonSpark.Activation;
using DragonSpark.Activation.Location;
using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;

namespace DragonSpark.Application.Setup
{
	public sealed class Instances : DelegatedSource<IServiceRepository>
	{
		public static ISource<IServiceRepository> Default { get; } = new Instances().ToSingletonScope();
		Instances() : base( () => new InstanceRepository( SingletonLocator.Default, Constructor.Default ) ) {}
	}
}