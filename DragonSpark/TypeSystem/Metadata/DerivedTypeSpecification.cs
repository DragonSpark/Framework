using DragonSpark.Specifications;
using System.Reflection;

namespace DragonSpark.TypeSystem.Metadata
{
	public class DerivedTypeSpecification : SpecificationBase<TypeInfo>
	{
		public static DerivedTypeSpecification Default { get; } = new DerivedTypeSpecification();
		DerivedTypeSpecification() {}

		public override bool IsSatisfiedBy( TypeInfo parameter ) => parameter.BaseType != typeof(object);
	}
}