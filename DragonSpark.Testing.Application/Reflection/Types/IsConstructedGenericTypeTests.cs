using DragonSpark.Reflection.Types;
using FluentAssertions;
using Xunit;

// ReSharper disable UnusedTypeParameter

namespace DragonSpark.Testing.Application.Reflection.Types
{
	public sealed class IsConstructedGenericTypeTests
	{
		sealed class Subject<T> {}

		interface ISubject<T> {}

		[Fact]
		void Verify()
		{
			var sut = IsConstructedGenericType.Default;
			sut.Get(typeof(Subject<object>)).Should().BeTrue();
			sut.Get(typeof(Subject<>)).Should().BeFalse();
		}

		[Fact]
		void VerifyInterface()
		{
			var sut = IsConstructedGenericType.Default;
			sut.Get(typeof(ISubject<object>)).Should().BeTrue();
			sut.Get(typeof(ISubject<>)).Should().BeFalse();
		}
	}
}