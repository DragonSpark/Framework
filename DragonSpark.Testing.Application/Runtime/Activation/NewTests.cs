using AutoFixture.Xunit2;
using FluentAssertions;
using DragonSpark.Reflection;
using DragonSpark.Runtime.Activation;
using Xunit;

namespace DragonSpark.Testing.Application.Runtime.Activation
{
	public class NewTests
	{
		[Theory, AutoData]
		public void Verify(int number)
		{
			var subject = I<Subject>.Default.New(number);
			subject.Number.Should()
			       .Be(number)
			       .And.Subject.Should()
			       .NotBeSameAs(New<int, Subject>.Default.Get(number));
		}

		[Theory, AutoData]
		public void Default(int number)
		{
			I<Subject>.Default.New(number).Number.Should().Be(number);
		}

		[Theory, AutoData]
		public void MultipleParameters(int number)
		{
			var subject = I<SubjectWithMultipleParameters>.Default.New(number);
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
			I<SubjectWithoutConstructor>.Default.New(6776).Should().NotBeNull();
		}

		[Fact]
		public void References()
		{
			var first  = I<SubjectWithoutConstructor>.Default.New(6776);
			var second = I<SubjectWithoutConstructor>.Default.New(6776);
			first.Should().NotBeSameAs(second);
		}
	}
}