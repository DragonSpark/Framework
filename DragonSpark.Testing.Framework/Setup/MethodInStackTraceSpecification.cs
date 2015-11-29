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

	class MethodInStackTraceSpecification : SpecificationBase<MethodInfo>
	{
		public MethodInStackTraceSpecification( MethodInfo context ) : base( context )
		{}

		protected override bool IsSatisfiedBy( object context )
		{
			var result = new System.Diagnostics.StackTrace().GetFrames()
				.Select( x => x.GetMethod() )
				.OfType<MethodInfo>()
				.Contains( Context );
			return result;
		}
	}
}