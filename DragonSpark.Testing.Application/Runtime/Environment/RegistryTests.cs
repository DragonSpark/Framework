using DragonSpark.Runtime.Environment;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Application.Runtime.Environment
{
	public sealed class RegistryTests
	{
		[Fact]
		void VerifyAdd()
		{
			var subject = new Registry<object>();

			var array = subject.Get();
			array.Open().Should().BeEmpty();
			var element = new object();
			subject.Execute(element);
			var result = subject.Get().Open();
			result.Should().Contain(element);
			result.Should().HaveCount(1);
		}

		[Fact]
		void VerifyAddRange()
		{
			var subject = new Registry<object>();

			var array = subject.Get();
			array.Open().Should().BeEmpty();
			var elements = new[] {new object(), new object(), new object()};
			subject.Execute(elements);
			var result = subject.Get().Open();
			result.Should().HaveCount(elements.Length);
			result.Should().Equal(elements);
		}
	}
}