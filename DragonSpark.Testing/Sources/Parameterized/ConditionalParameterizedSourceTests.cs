using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using Xunit;

namespace DragonSpark.Testing.Sources.Parameterized
{
	public class ConditionalParameterizedSourceTests
	{
		[Fact]
		public void Verify()
		{
			var trueInstance = new object();
			var falseInstance = new object();
			var sut = new ConditionalParameterizedSource<bool, object>( new DelegatedSpecification<bool>( b => b ), trueInstance.Shift, falseInstance.Shift );
			Assert.Same( trueInstance, sut.Get( true ) );
			Assert.Same( falseInstance, sut.Get( false ) );
		}
	}
}