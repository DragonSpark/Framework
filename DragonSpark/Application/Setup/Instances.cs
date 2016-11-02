using DragonSpark.Activation;
using DragonSpark.Activation.Location;
using DragonSpark.Sources;
using DragonSpark.Sources.Scopes;

namespace DragonSpark.Application.Setup
{
	public sealed class Instances : DelegatedSource<IServiceRepository>
	{
		public static Instances Default { get; } = new Instances();
		Instances() : base( Scopes.ToScopeDelegate( () => new InstanceRepository( SingletonLocator.Default, Constructor.Default ) ) ) {}
	}
}