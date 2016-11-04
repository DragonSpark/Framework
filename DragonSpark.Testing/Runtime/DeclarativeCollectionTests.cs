using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Testing.Objects;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public class DeclarativeCollectionTests
	{
		[Theory, AutoData]
		public void Equality( System.Collections.ObjectModel.Collection<Tuple<object>> sut )
		{
			var item = new object();
			var first = Tuple.Create( item );
			var second = Tuple.Create( item );

			Assert.False( sut.Contains( second ) );

			sut.Add( first );

			Assert.True( sut.Contains( second ) );
		}

		[Theory, AutoData]
		public void EqualityDictionary( Dictionary<Tuple<object>, object> sut )
		{
			var item = new object();
			var first = Tuple.Create( item );
			var second = Tuple.Create( item );

			sut.Add( first, item );

			var found = sut.TryGet( second );
			Assert.Same( item, found );
		}

		[Theory, AutoData]
		void Add( DeclarativeCollection<Class> sut, DeclarativeCollection coverage )
		{
			Assert.Equal( -1, sut.To<IList>().Add( new object() ) );
			Assert.NotSame( sut, coverage );
		}

		[Fact]
		public void Coverage()
		{
			var item = new object();
			IList<object> sut = new DeclarativeCollection { item };
			sut[0] = new object();
			Assert.NotSame( item, sut[0] );
		}
	}
}