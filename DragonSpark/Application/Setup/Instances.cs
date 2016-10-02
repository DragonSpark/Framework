using DragonSpark.Activation;
using DragonSpark.Activation.Location;
using DragonSpark.Sources;

namespace DragonSpark.Application.Setup
{
	public sealed class Instances : Scope<IServiceRepository>
	{
		public static ISource<IServiceRepository> Default { get; } = new Instances();
		Instances() : base( Factory.GlobalCache( () => new InstanceRepository( SingletonLocator.Default, Constructor.Default ) ) ) {}

		// public static T Get<T>( Type type ) => Default.Get().Get<T>( type );
	}
}