using AutoFixture.Xunit2;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Runtime.Activation;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	// ReSharper disable once TestFileNameSpaceWarning
	public class ObjectsTests
	{
		[Theory, AutoData]
		void AsTo(string message)
		{
			1.AsTo<string, object>(x => x, () => message).Should().Be(message);
			1.AsTo<string, object>(x => x).Should().BeNull();
		}

		[Theory, AutoData]
		public void AsToDefault(char[] expected)
		{
			3.AsTo<string, char[]>(x => x.ToCharArray(), expected.Self)
			 .Should()
			 .BeSameAs(expected);
		}

		[Fact]
		public void Accept()
		{
			var o = new object();
			o.Accept(123)
			 .Should()
			 .BeSameAs(o);
		}

		[Fact]
		public void AsInvalid()
		{
			Assert.Throws<InvalidOperationException>(() => 2.To<string>());
		}

		[Fact]
		public void GetValid()
		{
			Assert.Throws<InvalidOperationException>(() => new ServiceProvider().To<int>());
		}

		[Fact]
		public void NullIfEmpty()
		{
			var message = "Hello World!";
			message.NullIfEmpty()
			       .Should()
			       .Be(message);
			string.Empty.NullIfEmpty()
			      .Should()
			      .BeNull();
		}

		[Fact]
		public void Self()
		{
			var o = new object();
			o.Self()
			 .Should()
			 .BeSameAs(o);
		}

		[Fact]
		public void ToDictionary()
		{
			var dictionary = new[] {Pairs.Create("Hello", "World")}.ToDictionary();
			dictionary["Hello"]
				.Should()
				.Be("World");
		}

		[Fact]
		public void With()
		{
			var count = 0;
			count.With(_ => { count++; });
			count.Should()
			     .Be(1);
		}

		[Fact]
		public void YieldMetadata()
		{
			GetType()
				.Yield()
				.Where(x => x.Name != string.Empty)
				.Only()
				.Should()
				.Be(GetType());
		}
	}
}