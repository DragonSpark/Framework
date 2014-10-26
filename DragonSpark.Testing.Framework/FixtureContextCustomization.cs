using Ploeh.AutoFixture;

namespace DragonSpark.Testing.Framework
{
	public class FixtureContextCustomization : ICustomization
	{
		public void Customize( IFixture fixture )
		{
			Fixture.Current = fixture;
		}
	}
}