using AutoFixture.Xunit2;
using DragonSpark.Compose;
using DragonSpark.Runtime.Activation;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Runtime.Activation
{
	public class NewTests
	{
		[Theory, AutoData]
		public void Verify(int number)
		{
			var subject = Start.An.Extent<Subject>().New(number);
			subject.Number.Should().Be(number);
			subject.Should().NotBeSameAs(New<int, Subject>.Default.Get(number));
		}

		[Theory, AutoData]
		public void Default(int number)
		{
			Start.An.Extent<Subject>().New(number).Number.Should().Be(number);
		}

		[Theory, AutoData]
		public void MultipleParameters(int number)
		{
			var subject = Start.An.Extent<SubjectWithMultipleParameters>().New(number);
			subject.Another.Should().Be(4);
			subject.Number.Should().Be(number);
		}

		sealed class Subject
		{
			public Subject(int number) => Number = number;

			public int Number { get; }
		}

		sealed class SubjectWithoutConstructor {}

		sealed class SubjectWithMultipleParameters
		{
			public SubjectWithMultipleParameters(int number, int another = 4)
			{
				Number  = number;
				Another = another;
			}

			public int Number { get; }

			public int Another { get; }
		}

		[Fact]
		public void NoParameters()
		{
			Start.An.Extent<SubjectWithoutConstructor>().New(6776).Should().NotBeNull();
		}

		[Fact]
		public void References()
		{
			var first  = Start.An.Extent<SubjectWithoutConstructor>().New(6776);
			var second = Start.An.Extent<SubjectWithoutConstructor>().New(6776);
			first.Should().NotBeSameAs(second);
		}
	}
}