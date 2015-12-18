using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using DragonSpark.Testing.Framework;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.TestObjects;
using Xunit;

namespace DragonSpark.Testing.Setup
{
	public class AllTypesOfFactoryTests
	{
		[Theory, Test, SetupAutoData]
		public void Factory( AllTypesOfFactory sut )
		{
			var items = sut.Create<IInterface>();
			Assert.True( items.Any() );
			Assert.NotNull( items.FirstOrDefaultOfType<YetAnotherClass>() );
		} 
	}
}