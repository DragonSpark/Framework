using DragonSpark.Extensions;
using DragonSpark.Testing.TestObjects;
using System;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace DragonSpark.Testing.Extensions
{
	public class EnumerableExtensionsTests
	{
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

		[Theory, Framework.AutoData]
		void AsItem( object[] sut, object item )
		{
			var items = item.AsItem( sut );
			Assert.Equal( item, items.First() );
			Assert.Equal( sut.Last(), items.Last() );

			Assert.Single( item.AsItem(), item );
		}

		[Theory, Framework.AutoData]
		void Adding( object[] sut, object item )
		{
			var items = sut.Adding( item );
			Assert.Equal( item, items.Last() );
			Assert.Equal( sut.First(), items.First() );
		}

		[Theory, Framework.AutoData]
		public void TupleWith( Type[] types, string[] strings )
		{
			var tuple = types.TupleWith( strings ).ToList();
			Assert.Equal( tuple.Count, types.Length );
			Assert.Equal( tuple.Count, strings.Length );

			tuple.Apply( x =>
			{
				var index = tuple.IndexOf( x );
				Assert.Equal( types[index], x.Item1 );
				Assert.Equal( strings[index], x.Item2 );
			} );
		}

		[Theory, Framework.AutoData]
		void FirstOrDefaultOfType( Class first, ClassWithParameter second, string third, Derived fourth )
		{
			var items = new object[] { first, second, third, fourth };
			var sut = items.FirstOrDefaultOfType<string>();
			Assert.Equal( third, sut );
		}
	}
}
