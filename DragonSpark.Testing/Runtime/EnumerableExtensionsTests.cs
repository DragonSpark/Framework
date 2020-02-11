using AutoFixture.Xunit2;
using DragonSpark.Compose;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

// ReSharper disable PossibleMultipleEnumeration

namespace DragonSpark.Testing.Runtime
{
	public class EnumerableExtensionsTests
	{
		[Theory, AutoData]
		public void Prepend(IEnumerable<object> sut, object[] items)
		{
			var prepended = sut.Prepend(items);
			Assert.Equal(sut.Count() + items.Length, prepended.Count());
			Assert.True(sut.All(x => prepended.Contains(x)));
			Assert.True(items.All(x => prepended.Contains(x)));
		}

		[Theory, AutoData]
		public void PrependItem(object sut, IEnumerable<object> items)
		{
			var prepended = ExtensionMethods.Append(items, sut);
			Assert.Equal(items.Count() + 1, prepended.Count());
			Assert.Same(prepended.Last(), sut);
		}

		[Theory, AutoData]
		public void EachWithFunc(IEnumerable<object> sut)
		{
			var                copy   = sut.ToList();
			Func<object, bool> action = copy.Remove;
			sut.ForEach(action);
			copy.Should()
			    .BeEmpty();
			//Assert.All(results, b => b.IsFalse(() => { throw new InvalidOperationException("was not true."); }));
		}

		[Theory, AutoData]
		void Adding(object[] sut, object item)
		{
			var items = sut.Append(item);
			Assert.Equal(item, items.Last());
			Assert.Equal(sut.First(), items.First());
		}

		[Fact]
		public void All()
		{
			new[] {true, true}.All()
			                  .Should()
			                  .BeTrue();
			new[] {true, false, false, false}.All()
			                                 .Should()
			                                 .BeFalse();
		}

		[Fact]
		public void AnyFalse()
		{
			Assert.True(new[] {true, true, true, false}.AnyFalse());
			Assert.False(new[] {true, true, true, true}.AnyFalse());
		}

		[Fact]
		public void AnyTrue()
		{
			Assert.True(new[] {true, false, false, false}.AnyTrue());
			Assert.False(new bool[4].AnyTrue());
		}

		[Fact]
		public void AppendParams()
		{
			1.Yield().Append(2, 3).Should().BeEquivalentTo(new[] {1, 2, 3});
		}

		[Fact]
		public void Introduce()
		{
			var sut = new[] {new Func<int, bool>(i => i == 7)};
			Assert.True(sut.Introduce(7).Only());
		}

		[Fact]
		public void IntroduceTuple()
		{
			new[] {1}.Introduce(2).Only().Should().BeEquivalentTo((1, 2));
		}

		[Fact]
		public void IntroduceTupleWhere()
		{
			new[] {1}.Introduce(2, x => x.Item1 == 1)
			         .Only()
			         .Should()
			         .Be(1);
		}

		[Fact]
		public void Only()
		{
			var sut = new[] {2, 3, 5}.Where(i => i == 3).Only();
			Assert.Equal(3, sut);
		}

		[Fact]
		public void PrependBasic()
		{
			const int sut   = 3;
			var       items = sut.Yield().Prepend(4, 5);
			Assert.Contains(4, items);
			Assert.Contains(sut, items);
		}
	}
}