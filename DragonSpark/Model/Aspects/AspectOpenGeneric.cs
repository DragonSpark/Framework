using DragonSpark.Reflection.Types;

namespace DragonSpark.Model.Aspects
{
	public sealed class AspectOpenGeneric : OpenGeneric
	{
		public static AspectOpenGeneric Default { get; } = new AspectOpenGeneric();

		AspectOpenGeneric() : base(typeof(IAspect<,>)) {}
	}
}