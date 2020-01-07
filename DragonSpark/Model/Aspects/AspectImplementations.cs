using DragonSpark.Reflection.Types;

namespace DragonSpark.Model.Aspects
{
	public sealed class AspectImplementations : GenericImplementations
	{
		public static AspectImplementations Default { get; } = new AspectImplementations();

		AspectImplementations() : base(typeof(IAspect<,>)) {}
	}
}