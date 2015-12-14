using System.Linq;
using System.Reflection;
using DragonSpark.Runtime.Specifications;

namespace DragonSpark.Testing.Framework.Setup
{
	/*public class ServiceLocation : FixedValue<IServiceLocator>, IServiceLocation
	{
		public bool IsAvailable => Locator != null;

		public IServiceLocator Locator => Item;
	}*/

	class MethodInStackTraceContextAwareSpecification : ContextAwareSpecificationBase<MethodInfo>
	{
		public MethodInStackTraceContextAwareSpecification( MethodInfo context ) : base( context )
		{}

		protected override bool IsSatisfiedByParameter( MethodInfo parameter )
		{
			var result = new System.Diagnostics.StackTrace().GetFrames()
				.Select( x => x.GetMethod() )
				.OfType<MethodInfo>()
				.Contains( Context );
			return result;
		}
	}
}