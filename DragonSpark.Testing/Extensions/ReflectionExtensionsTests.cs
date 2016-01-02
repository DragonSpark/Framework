using DragonSpark.Extensions;
using DragonSpark.Testing.Objects;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class ReflectionExtensionsTests
	{
		[Theory, AutoData]
		void GenericInvoke( Class @class )
		{
			typeof(Static).InvokeGenericAction( "Assign", new []{ typeof(Class) }, null );
			
			Assert.Null( Static.Instance );
			
			typeof(Static).InvokeGenericAction( "Assign", new []{ typeof(Class) }, @class );

			Assert.Equal( @class, Static.Instance );
		}
	}
}