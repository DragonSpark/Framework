using DragonSpark.Reflection.Types;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Reflection.Types
{
	public sealed class IsGenericTypeDefinitionTests
	{
		// ReSharper disable once UnusedTypeParameter
		sealed class Subject<T> {}

		[Fact]
		public void Verify()
		{
			var sut = IsGenericTypeDefinition.Default;
			sut.Get(typeof(Subject<object>)).Should().BeFalse();
			sut.Get(typeof(Subject<>)).Should().BeTrue();
		}
	}
}