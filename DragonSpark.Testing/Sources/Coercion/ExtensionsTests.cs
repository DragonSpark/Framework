using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.Testing.Objects;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using Xunit;

namespace DragonSpark.Testing.Sources.Coercion
{
	public class ExtensionsTests
	{
		[Fact]
		public void Apply()
		{
			var instance = ServiceRelay.Default.Create( typeof(Class), new SpecimenContext( new Fixture() ) );
			Assert.IsType<Class>( instance );
		}
	}
}