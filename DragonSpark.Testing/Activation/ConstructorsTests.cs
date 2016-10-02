using DragonSpark.Activation;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Testing.Activation
{
	public class ConstructorsTests
	{
		[Theory, AutoData]
		void CachesAsExpected( Constructors constructors, int number )
		{
			var parameter = new ConstructTypeRequest( typeof(ConstructedItem), number );
			var first = constructors.Get( parameter );
			Assert.Same( first, constructors.Get( parameter ) );
			Assert.Equal( number, new ConstructedItem( number ).Number );
		}

		class ConstructedItem
		{
			public ConstructedItem( int number )
			{
				Number = number;
			}

			public int Number { get; set; }
		}
	}
}