using DragonSpark.Extensions;
using DragonSpark.Testing.TestObjects;
using Xunit;
using Xunit.Extensions;

namespace DragonSpark.Testing.Extensions
{
	public class ReflectionExtensionsTests
	{
		[Theory, Framework.AutoDataCustomization]
		void GenericInvoke( Class @class )
		{
			typeof(Static).InvokeGenericAction( "Assign", new []{ typeof(Class) }, null );
			
			Assert.Null( Static.Instance );
			
			typeof(Static).InvokeGenericAction( "Assign", new []{ typeof(Class) }, @class );

			Assert.Equal( @class, Static.Instance );
		}
	}
}