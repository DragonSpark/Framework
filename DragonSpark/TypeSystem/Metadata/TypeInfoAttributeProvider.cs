using System.Reflection;

namespace DragonSpark.TypeSystem.Metadata
{
	public class TypeInfoAttributeProvider : MemberInfoAttributeProvider
	{
		public TypeInfoAttributeProvider( TypeInfo info ) : base( info, DerivedTypeSpecification.Default.IsSatisfiedBy( info ) ) {}
	}
}