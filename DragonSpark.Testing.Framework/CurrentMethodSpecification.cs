using System.Reflection;
using DragonSpark.Runtime;

namespace DragonSpark.Testing.Framework
{
	class CurrentMethodSpecification : AnySpecification
	{
		public CurrentMethodSpecification( MethodInfo method  ) : base( new MemberInfoSpecification( method ), new MethodInStackTraceSpecification( method ) )
		{}
	}
}