using DragonSpark.Reflection.Types;

namespace DragonSpark.Aspects
{
	public sealed class AspectImplementations : GenericImplementations
	{
		public static AspectImplementations Default { get; } = new AspectImplementations();

		AspectImplementations() : base(typeof(IAspect<,>)) {}
	}
}