using DragonSpark.Activation;
using DragonSpark.Extensions;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Activation
{
	public sealed class ConstructorTests
	{
		[Theory, AutoData]
		public void CreateDefault()
		{
			var type = typeof(bool);
			var result = Constructor.Default.Get<object>( type );
			Assert.Equal( false, result );
		}
	}
}