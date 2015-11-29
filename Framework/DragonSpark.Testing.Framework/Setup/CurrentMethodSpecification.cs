using System.Reflection;
using DragonSpark.Runtime.Specifications;

namespace DragonSpark.Testing.Framework.Setup
{
	class CurrentMethodSpecification : AnySpecification
	{
		public CurrentMethodSpecification( MethodInfo method  ) : base( new MemberInfoSpecification( method ), new MethodInStackTraceSpecification( method ) )
		{}
	}
}