using DragonSpark.Runtime.Activation;
using FluentAssertions;
using Xunit;

// ReSharper disable All

namespace DragonSpark.Testing.Application.Runtime.Activation
{
	public sealed class SingletonTests
	{
		sealed class Subject
		{
			public static Subject Default { get; } = new Subject();

			Subject() {}
		}

		sealed class Class {}

		[Fact]
		public void Reference()
		{
			Singleton<Subject>.Default.Get()
			                  .Should()
			                  .NotBeNull()
			                  .And.Subject.Should()
			                  .BeSameAs(Singleton<Subject>.Default.Get());
		}

		[Fact]
		public void Undeclared()
		{
			Singleton<Class>.Default.Get().Should().BeNull();
		}
	}
}