using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Runtime;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.Objects;
using Ploeh.AutoFixture.Xunit2;
using System.Collections;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public class CollectionTests
	{
		[Theory, MoqAutoData]
		void Add( Collection<Class> sut )
		{
			Assert.Equal( -1, sut.To<IList>().Add( new object() ) );
		}

		[Theory, AutoData]
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