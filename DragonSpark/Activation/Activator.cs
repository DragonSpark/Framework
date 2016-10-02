using DragonSpark.Activation.Location;

namespace DragonSpark.Activation
{
	public sealed class Activator : CompositeActivator
	{
		public static IActivator Default { get; } = new Activator();
		Activator() : base( SingletonLocator.Default, Constructor.Default ) {}
	}
}