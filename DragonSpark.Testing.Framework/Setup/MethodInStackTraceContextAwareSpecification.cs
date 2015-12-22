using DragonSpark.Runtime.Specifications;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Setup
{
	class MethodInStackTraceContextAwareSpecification : ContextAwareSpecificationBase<MethodInfo>
	{
		public MethodInStackTraceContextAwareSpecification( MethodInfo context ) : base( context )
		{}

		protected override bool IsSatisfiedByParameter( MethodInfo parameter )
		{
			var result = new StackTrace( false ).GetFrames()
				.Select( x => x.GetMethod() )
				.OfType<MethodInfo>()
				.Contains( Context );
			return result;
		}
	}
}