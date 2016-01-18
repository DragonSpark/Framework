using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Runtime;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.Objects;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public class CollectionTests
	{
		[Theory, Framework.Setup.AutoData]
		public void Equality( System.Collections.ObjectModel.Collection<Tuple<object>> sut )
		{
			var item = new object();
			var first = Tuple.Create( item );
			var second = Tuple.Create( item );

			Assert.False( sut.Contains( second ) );

			sut.Add( first );

			Assert.True( sut.Contains( second ) );
		}

		[Fact]
		public void BasicEquality()
		{
			Assert.Equal( new EqualityList { true }, new EqualityList { true } );
			Assert.NotEqual( new EqualityList { true }, new EqualityList { false } );
		}

		[Fact]
		public void NumberEquality()
		{
			object item = new object();
			Assert.Equal( new EqualityList { item, 1 }, new EqualityList { item, 1 } );
			Assert.NotEqual( new EqualityList { item, 1 }, new EqualityList { item, 3 } );
		}

		[Theory, Framework.Setup.AutoData]
		public void Array( object[] items )
		{
			var first = new EqualityList( items.ToArray() );
			var second = new EqualityList( items.ToArray() );
			Assert.Equal( first.GetHashCode(), second.GetHashCode() );
			Assert.NotEqual( items.ToArray().GetHashCode(), items.ToArray().GetHashCode() );
		}

		[Theory, Framework.Setup.AutoData]
		public void EqualityDictionary( Dictionary<Tuple<object>, object> sut )
		{
			var item = new object();
			var first = Tuple.Create( item );
			var second = Tuple.Create( item );

			sut.Add( first, item );

			var found = sut.TryGet( second );
			Assert.Same( item, found );
		}

		[Theory, Framework.Setup.AutoData]
		void Add( Collection<Class> sut )
		{
			Assert.Equal( -1, sut.To<IList>().Add( new object() ) );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void Cover( ModuleInfo one, ModuleInfo two, Collection<ModuleInfo> sut )
		{
			( (IList)sut ).Insert( 0, one );
			Assert.Same( sut.First(), one );

			Assert.False( ( (IList)sut ).IsFixedSize );

			// Assert.Throws<ArgumentException>( () => sut.Insert( 0, new object() ) );
			( (IList)sut ).Insert( 0, (object)one );

			Assert.True( sut.Contains( one ) );

			Assert.Equal( 0, ( (IList)sut ).IndexOf( one ) );
			Assert.Equal( -1, ( (IList)sut ).IndexOf( new object() ) );
			Assert.Equal( 0, ( (IList)sut ).IndexOf( (object)one ) );
			Assert.True( sut.Remove( one ) );
			sut.To<IList>().With( x =>
			{
				x.Add( one );
				x[0] = two;
				Assert.Same( x[0], two );
				Assert.True( x.Contains( one ) );
				x.Remove( one );
				Assert.False( x.Contains( one ) );
			} );
			Assert.False( sut.IsReadOnly );
			Assert.Equal( sut.Count, sut.Count() );

			sut.To<ICollection>().With( x =>
			{
				var array = new ModuleInfo[x.Count];
				x.CopyTo( array, 0 );
				Assert.Equal( sut, array );
			} );

			( (IList)sut ).Insert( 0, one );
			( (IList)sut ).RemoveAt( 0 );

			var actual = new ModuleInfo[sut.Count];
			sut.CopyTo( actual, 0 );
			Assert.Equal( sut, actual );

			Assert.False( ( (ICollection)sut ).IsSynchronized );
			Assert.NotNull( ( (ICollection)sut ).SyncRoot );
			Assert.NotNull( sut.To<IEnumerable>().GetEnumerator() );
			sut.Clear();
			Assert.Equal( 0, sut.Count );
		}
	}
}