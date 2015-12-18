using System.Reflection;

namespace DragonSpark.Runtime.Specifications
{
	public class MemberInfoSpecification : AnySpecification
	{
		public MemberInfoSpecification( MemberInfo member ) : base( new EqualityContextAwareSpecification( member ), new TypeContextAwareSpecification( member.DeclaringType ) )
		{}
	}
}