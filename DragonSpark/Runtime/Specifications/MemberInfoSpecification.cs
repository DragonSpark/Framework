using System.Reflection;

namespace DragonSpark.Runtime.Specifications
{
	public class MemberInfoSpecification : AnySpecification
	{
		public MemberInfoSpecification( MemberInfo member ) : base( new EqualitySpecification( member ), new TypeSpecification( member.DeclaringType ) )
		{}
	}
}