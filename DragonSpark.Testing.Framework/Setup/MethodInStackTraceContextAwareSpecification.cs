using DragonSpark.Runtime.Specifications;
using DragonSpark.Windows.Runtime;
using System.Reflection;

namespace DragonSpark.Testing.Framework.Setup
{
	public class CurrentMethodValue : ThreadLocalValue<MethodInfo>
	{
		public CurrentMethodValue() : base( typeof(CurrentMethodValue).AssemblyQualifiedName )
		{ }
	}

	class MethodInStackTraceContextAwareSpecification : ContextAwareSpecificationBase<MethodInfo, object>
	{
		public MethodInStackTraceContextAwareSpecification( MethodInfo context ) : base( context )
		{}

		protected override bool IsSatisfiedByParameter( object parameter )
		{
			var result = new CurrentMethodValue().Item == Context;
			return result;
			
			/*var result = new StackTrace( false ).GetFrames()
				.Select( x => x.GetMethod() )
				.OfType<MethodInfo>()
				.Contains( Context );
			return result;*/

			/*var result = AmbientValues.Get<IFixture>( Context ).With( fixture => fixture.Item<CurrentMethodCustomization>().IsExecuting );
			return result;*/
		}
	}
}