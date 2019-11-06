using System;
using FluentAssertions;
using DragonSpark.Reflection;
using Xunit;

namespace DragonSpark.Testing.Application.Reflection
{
	public sealed class AttributeTests
	{
		sealed class Extend : Root {}

		[Subject]
		class Root {}

		[AttributeUsage(AttributeTargets.Class)]
		sealed class SubjectAttribute : Attribute {}

		[Fact]
		void Verify()
		{
			var provider = typeof(Extend);
			LocalAttribute<SubjectAttribute>.Default.Get(provider)
			                                .Should()
			                                .BeNull();

			var attribute = Attribute<SubjectAttribute>.Default.Get(provider);
			attribute.Should().NotBeNull();
			Attribute<SubjectAttribute>.Default.Get(provider).Should().BeSameAs(attribute);
		}
	}
}