using DragonSpark.Extensions;
using DragonSpark.Testing.Framework.Setup;
using DragonSpark.Testing.Objects;
using Ploeh.AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Extensions
{
	public class ContinuationExceptionTests
	{
		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void Each( IEnumerable<object> sut )
		{
			var count = 0;
			Action<object> action = o => count++;
			sut.Each( action );
			Assert.Equal( sut.Count(), count );
		}

		[Theory, Framework.Setup.AutoData]
		public void NullIfEmpty( IEnumerable<object> sut )
		{
			Assert.NotNull( sut.NullIfEmpty() );

			Assert.Null( Enumerable.Empty<object>().NullIfEmpty() );
		}


		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void Prepend( IEnumerable<object> sut, object[] items )
		{
			var prepended = sut.Prepend( items );
			Assert.Equal( sut.Count() + items.Length, prepended.Count() );
			Assert.True( sut.All( x => prepended.Contains( x ) ) );
			Assert.True( items.All( x => prepended.Contains( x ) ) );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void PrependItem( object sut, IEnumerable<object> items )
		{
			var prepended = sut.Prepend( items );
			Assert.Equal( items.Count() + 1, prepended.Count() );
			Assert.Same( prepended.Last(), sut );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void EachWithFunc( IEnumerable<object> sut )
		{
			var copy = sut.ToList();
			Func<object, bool> action = copy.Remove;
			var results = sut.Each( action );
			Assert.All( results,  b => b.IsFalse( () => { throw new InvalidOperationException( "was not true." ); } ) );
		}

		[Fact]
		void Prioritize()
		{
			var items = new object[] { new LowPriority(), new NormalPriority(), new HighPriority() }.Prioritize().ToArray();
			Assert.IsType<HighPriority>( items.First() );
			Assert.IsType<LowPriority>( items.Last() );
		}

		[Fact]
		void PrioritizePredicate()
		{
			var items = new object[] { new LowPriority(), new NormalPriority(), new HighPriority() }.Prioritize( x => x.GetType().GetAttribute<PriorityAttribute>() ).ToArray();
			Assert.IsType<HighPriority>( items.First() );
			Assert.IsType<LowPriority>( items.Last() );
		}

		[Fact]
		void PrioritizeNormalObject()
		{
			var items = new object[] { new LowPriority(), new Class() }.Prioritize().ToArray();
			Assert.IsType<Class>( items.First() );
			Assert.IsType<LowPriority>( items.Last() );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		void AsItem( object[] sut, object item )
		{
			var items = item.Append( sut );
			Assert.Equal( item, items.First() );
			Assert.Equal( sut.Last(), items.Last() );

			var asItem = item.ToItem();
			Assert.Single( asItem, item );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		void Adding( object[] sut, object item )
		{
			var items = sut.Append( item );
			Assert.Equal( item, items.Last() );
			Assert.Equal( sut.First(), items.First() );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void TupleWith( Type[] types, string[] strings )
		{
			var tuple = types.TupleWith( strings ).ToList();
			Assert.Equal( tuple.Count, types.Length );
			Assert.Equal( tuple.Count, strings.Length );

			tuple.Each( x =>
			{
				var index = tuple.IndexOf( x );
				Assert.Equal( types[index], x.Item1 );
				Assert.Equal( strings[index], x.Item2 );
			} );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		void FirstOrDefaultOfType( Class first, ClassWithParameter second, string third, Derived fourth )
		{
			var items = new object[] { first, second, third, fourth };
			var sut = items.FirstOrDefaultOfType<string>();
			Assert.Equal( third, sut );
		}
	}
}
