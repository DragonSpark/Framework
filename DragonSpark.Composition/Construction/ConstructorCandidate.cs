using System.Reflection;

namespace DragonSpark.Composition.Construction;

public readonly record struct ConstructorCandidate(ConstructorInfo Constructor, ParameterInfo[] Parameters)
{
	public ConstructorCandidate(ConstructorInfo constructor) : this(constructor, constructor.GetParameters()) {}

	public void Deconstruct(out ConstructorInfo constructor, out ParameterInfo[] parameters)
	{
		constructor = Constructor;
		parameters  = Parameters;
	}
}
