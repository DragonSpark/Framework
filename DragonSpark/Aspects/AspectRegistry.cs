using DragonSpark.Runtime.Environment;

namespace DragonSpark.Aspects
{
	public sealed class AspectRegistry : SystemRegistry<IRegistration>
	{
		public static AspectRegistry Default { get; } = new AspectRegistry();

		AspectRegistry() {}
	}
}