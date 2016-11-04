using DragonSpark.Extensions;
using DragonSpark.Testing.Objects;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Type = System.Type;

namespace DragonSpark.Testing.Extensions
{
	[SuppressMessage( "ReSharper", "PossibleMultipleEnumeration" )]
	public class EnumerableExtensionsTests
	{
		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		public void Each( IEnumerable<object> sut )
		{
			var count = 0;
			Action<object> action = o => count++;
			sut.Each( action );
			Assert.Equal( sut.Count(), count );
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

		[Fact]
		public void Introduce()
		{
			var sut = new[] { new Func<int, bool>( i => i == 7 ) };
			Assert.True( sut.Introduce( 7 ).Only() );
		}

		[Fact]
		public void Only()
		{
			var sut = new[] { 2, 3, 5 }.ToImmutableArray().Only( i => i == 3 );
			Assert.Equal( sut, 3 );
		}

		[Fact]
		public void Fixed()
		{
			var sut = new[] { 2, 3, 5 }.Hide().Fixed( 4, 15 );
			Assert.Contains( 3, sut );
			Assert.Contains( 15, sut );
		}

		[Fact]
		public void PrependBasic()
		{
			const int sut = 3;
			var items = sut.Prepend( 4, 5 );
			Assert.Contains(4, items );
			Assert.Contains(sut, items );
		}

		[Fact]
		public void IntroduceImmutableArray()
		{
			var sut = new[] { 5 }.ToImmutableArray().Introduce( true );
			var only = sut.Only();
			Assert.Equal( 5, only.Item1 );
			Assert.Equal( true, only.Item2 );
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
			var low = new LowPriority();
			var items = new object[] { low, new NormalPriority(), new HighPriority() }.Prioritize().ToArray();
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
			var tuple = types.Tuple( strings ).ToList();
			Assert.Equal( tuple.Count, types.Length );
			Assert.Equal( tuple.Count, strings.Length );

			tuple.Each( x =>
			{
				var index = tuple.IndexOf( x );
				Assert.Equal( types[index], x.Item1 );
				Assert.Equal( strings[index], x.Item2 );
			} );
		}

		[Fact]
		public void AnyTrue()
		{
			Assert.True( new[] { true, false, false, false }.AnyTrue() );
			Assert.False( new[] { false, false, false, false }.AnyTrue() );
		}

		[Fact]
		public void AnyFalse()
		{
			Assert.True( new[] { true, true, true, false }.AnyFalse() );
			Assert.False( new[] { true, true, true, true }.AnyFalse() );
		}

		[Theory, Ploeh.AutoFixture.Xunit2.AutoData]
		void FirstOrDefaultOfType( Class first, ClassWithParameter second, string third, Derived fourth )
		{
			var items = new object[] { first, second, third, fourth };
			var sut = items.FirstOrDefaultOfType<string>();
			Assert.Equal( third, sut );
		}

		[Fact]
		public void SelectAssigned()
		{
			var numbers = new int?[] { 6, 7, null, 76 };
			var assigned = numbers.SelectAssigned( i => i ).Fixed();
			Assert.Equal( 3, assigned.Length );
			Assert.Equal( new [] { 6, 7, 76 }, assigned );
		}
	}
}
