using System.Reflection;
using DragonSpark.Specifications;

namespace DragonSpark.TypeSystem.Metadata
{
	public class DerivedMethodSpecification : SpecificationBase<MethodInfo>
	{
		public static DerivedMethodSpecification Default { get; } = new DerivedMethodSpecification();
		DerivedMethodSpecification() {}

		public override bool IsSatisfiedBy( MethodInfo parameter ) => 
			parameter.GetRuntimeBaseDefinition().DeclaringType != parameter.DeclaringType;
	}
}