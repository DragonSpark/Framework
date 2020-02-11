using DragonSpark.Runtime;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Runtime
{
	public class DisposableTests
	{
		[Fact]
		public void Verify()
		{
			var called = false;
			using (new Disposable(() => called = true))
			{
				called.Should()
				      .BeFalse();
			}

			called.Should()
			      .BeTrue();
		}
	}
}