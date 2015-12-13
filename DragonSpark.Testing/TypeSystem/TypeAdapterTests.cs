using DragonSpark.TypeSystem;
using Ploeh.AutoFixture.Xunit2;
using System.Collections.Generic;
using Xunit;

namespace DragonSpark.Testing.TypeSystem
{
	public class TypeAdapterTests
	{
		[Fact]
		public void EnumerableType()
		{
			var item = new TypeAdapter( typeof(List<int>) ).GetEnumerableType();
			Assert.Equal( typeof(int), item );
		}

		[Theory, AutoData]
		public void Qualify( int item )
		{
			var qualified = new TypeAdapter( typeof(Casted) ).Qualify( item );
			Assert.Equal( item, Assert.IsType<Casted>( qualified ).Item );
		}

		[Fact]
		public void IsInstanceOfType()
		{
			var adapter = new TypeAdapter( typeof(Casted) );
			Assert.True( adapter.IsInstanceOfType( new Casted( 6776 ) ) );
		}

		class Casted
		{
			public Casted( int item )
			{
				Item = item;
			}

			public int Item { get; }

			public static implicit operator Casted( int item )
			{
				return new Casted( item );
			}
		}
	}
}