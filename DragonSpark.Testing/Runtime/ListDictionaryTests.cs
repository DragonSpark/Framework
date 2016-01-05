using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Testing.Framework.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public class ListDictionaryTests
	{
		[Theory, AutoData]
		public void Cover( Guid key, string one, string two, ListDictionary<Guid, string> sut )
		{
			sut.Add( key, one );
			sut.Add( key, two );

			sut.Clear();

			Assert.Equal( 0, sut.Count );
			Assert.NotNull( sut.GetEnumerator() );

			sut.Add( key, one );
			sut.Add( key, two );
			sut.As<ICollection<KeyValuePair<Guid, IList<string>>>>( pairs =>
			{
				Assert.True( pairs.Remove( pairs.First() ) );
				Assert.False( pairs.Remove( new KeyValuePair<Guid, IList<string>>() ) );
				Assert.False( pairs.IsReadOnly );
				var array = new KeyValuePair<Guid, IList<string>>[ pairs.Count ];
				pairs.CopyTo( array, 0 );
				Assert.Equal( pairs.ToArray(), array.ToArray() );

				Assert.False( pairs.Contains( new KeyValuePair<Guid, IList<string>>() ) );
				pairs.Add( new KeyValuePair<Guid, IList<string>>( Guid.NewGuid(), "Hello".ToItem().ToList() )  );
			} );

			sut.To<IDictionary<Guid, IList<string>>>().With( dictionary =>
			{
				Assert.NotNull( dictionary.Values );
				IList<string> list;
				Assert.True( dictionary.TryGetValue( key, out list ) );
				dictionary.Add( Guid.NewGuid(), new List<string>() );
			} );

			sut.Clear();

			sut[key] = new List<string>();
			Assert.False( sut[key].Any() );
			Assert.True( sut.Remove( key ) );
		} 
	}
}