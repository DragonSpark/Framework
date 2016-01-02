using DragonSpark.Runtime.Specifications;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Runtime.Specifications
{
	public class AllSpecificationTests
	{
		[Fact]
		public void All()
		{
			var sut = new AllSpecification( new[] { true, true, true }.Select( x => new FixedSpecification( x ) ).Cast<ISpecification>().ToArray() );
			Assert.True( sut.IsSatisfiedBy( null ) );
		}

		[Fact]
		public void AllNot()
		{
			var sut = new AllSpecification( new[] { true, true, false }.Select( x => new FixedSpecification( x ) ).Cast<ISpecification>().ToArray() );
			Assert.False( sut.IsSatisfiedBy( null ) );
		}
	}
}