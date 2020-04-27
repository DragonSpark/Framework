using DragonSpark.Runtime.Environment;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Runtime.Environment
{
	public sealed class PrimaryAssemblyTests
	{
		[Fact]
		public void Verify()
		{
			PrimaryAssembly.Default.Get().Should().BeSameAs(GetType().Assembly);
		}
	}
}