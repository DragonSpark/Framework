using System.Reflection;

namespace DragonSpark.Composition.Construction
{
	public readonly struct ConstructorCandidate
	{
		public ConstructorCandidate(ConstructorInfo constructor)
			: this(constructor, constructor.GetParameters()) {}

		public ConstructorCandidate(ConstructorInfo constructor, ParameterInfo[] parameters)
		{
			Constructor = constructor;
			Parameters  = parameters;
		}

		public ConstructorInfo Constructor { get; }

		public ParameterInfo[] Parameters { get; }

		public void Deconstruct(out ConstructorInfo constructor, out ParameterInfo[] parameters)
		{
			constructor = Constructor;
			parameters  = Parameters;
		}
	}
}