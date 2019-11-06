using DragonSpark.Reflection.Types;

namespace DragonSpark.Aspects
{
	public sealed class AspectImplementationArguments : GenericImplementationArguments
	{
		public static AspectImplementationArguments Default { get; } = new AspectImplementationArguments();

		AspectImplementationArguments() : base(AspectImplementations.Default) {}
	}
}