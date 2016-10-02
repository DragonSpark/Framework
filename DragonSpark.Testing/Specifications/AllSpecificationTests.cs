using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Specifications
{
	public class AllSpecificationTests
	{
		[Fact]
		public void All()
		{
			var sut = new AllSpecification( new[] { true, true, true }.Select( x => new SuppliedSpecification( x ) ).ToArray() );
			Assert.True( sut.IsSatisfiedBy( Defaults.Parameter ) );
		}

		[Fact]
		public void AllNot()
		{
			var sut = new AllSpecification( new[] { true, true, false }.Select( x => new SuppliedSpecification( x ) ).ToArray() );
			Assert.False( sut.IsSatisfiedBy( Defaults.Parameter ) );
		}
	}
}