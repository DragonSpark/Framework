using System.Reflection;

namespace DragonSpark.TypeSystem.Metadata
{
	public class MethodInfoAttributeProvider : MemberInfoAttributeProvider
	{
		public MethodInfoAttributeProvider( MethodInfo method ) : this( method, method ) {}

		public MethodInfoAttributeProvider( MemberInfo member, MethodInfo method ) : base( member, DerivedMethodSpecification.Default.IsSatisfiedBy( method ) ) {}
	}
}