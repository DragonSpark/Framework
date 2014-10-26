using DragonSpark.Extensions;
using DragonSpark.Testing.TestObjects;
using Xunit;
using Xunit.Extensions;

namespace DragonSpark.Testing.Extensions
{
	public class ReflectionExtensionsTests
	{
		[Theory, Framework.AutoData]
		void GenericInvoke( Class @class )
		{
			typeof(Static).GenericInvoke( "Assign", new []{ typeof(Class) }, null );
			
			Assert.Null( Static.Instance );
			
			typeof(Static).GenericInvoke( "Assign", new []{ typeof(Class) }, @class );

			Assert.Equal( @class, Static.Instance );
		}
	}
}