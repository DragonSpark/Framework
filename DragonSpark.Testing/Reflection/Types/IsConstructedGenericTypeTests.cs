using DragonSpark.Reflection.Types;
using FluentAssertions;
using Xunit;

// ReSharper disable UnusedTypeParameter

namespace DragonSpark.Testing.Reflection.Types
{
	public sealed class IsConstructedGenericTypeTests
	{
		sealed class Subject<T> {}

		interface ISubject<T> {}

		[Fact]
		public void Verify()
		{
			var sut = IsConstructedGenericType.Default;
			sut.Get(typeof(Subject<object>)).Should().BeTrue();
			sut.Get(typeof(Subject<>)).Should().BeFalse();
		}

		[Fact]
		public void VerifyInterface()
		{
			var sut = IsConstructedGenericType.Default;
			sut.Get(typeof(ISubject<object>)).Should().BeTrue();
			sut.Get(typeof(ISubject<>)).Should().BeFalse();
		}
	}
}