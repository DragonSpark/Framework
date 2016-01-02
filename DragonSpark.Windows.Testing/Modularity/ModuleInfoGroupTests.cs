using System;
using System.Collections;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Modularity;
using DragonSpark.Windows.Modularity;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace DragonSpark.Windows.Testing.Modularity
{
	public class ModuleInfoGroupTests
	{
		[Fact]
		public void ShouldForwardValuesToModuleInfo()
		{
			var group = new DynamicModuleInfoGroup();
			group.Ref = "MyCustomGroupRef";
			var moduleInfo = new DynamicModuleInfo();
			Assert.Null(moduleInfo.Ref);

			group.Add(moduleInfo);

			Assert.Equal(group.Ref, moduleInfo.Ref);
		}

		[Theory, AutoData]
		public void Cover( ModuleInfo one, ModuleInfo two, ModuleInfoGroup sut )
		{
			sut.Insert( 0, one );
			Assert.Same( sut.First(), one );

			Assert.False(sut.IsFixedSize);

			Assert.Throws<ArgumentException>( () => sut.Insert( 0, new object() ) );
			sut.Insert( 0, (object)one );

			Assert.True( sut.Contains( one ) );
			
			Assert.Equal( 0, sut.IndexOf(one) );
			Assert.Equal( 0, sut.IndexOf( (object)one ) );
			Assert.True( sut.Remove( one )  );
			sut.To<IList>().With( x =>
			{
				x.Add( one );
				x[0] = two;
				Assert.Same( x[0], two );
				Assert.True( x.Contains( one ) );
				x.Remove( one );
				Assert.False( x.Contains( one ) );
				Assert.Throws<ArgumentException>( () => x.Contains( new object() ) );
			} );
			Assert.False( sut.IsReadOnly );
			Assert.Equal( sut.Count, sut.Count() );

			sut.To<ICollection>().With( x =>
			{
				var array = new ModuleInfo[x.Count];
				x.CopyTo( array, 0 );
				Assert.Equal( sut, array );
			} );

			sut.Insert( 0, one );
			sut.RemoveAt( 0 );

			var actual = new ModuleInfo[sut.Count];
			sut.CopyTo( actual, 0 );
			Assert.Equal( sut, actual );

			Assert.False( sut.IsSynchronized );
			Assert.NotNull( sut.SyncRoot );
			Assert.NotNull( sut.To<IEnumerable>().GetEnumerator() );
			sut.Clear();
			Assert.Equal( 0, sut.Count );
		}
	}
}
