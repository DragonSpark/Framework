using DragonSpark.Runtime.Environment;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Application.Runtime.Environment
{
	public sealed class PrimaryAssemblyTests
	{
		[Fact]
		void Verify()
		{
			PrimaryAssembly.Default.Get().Should().BeSameAs(GetType().Assembly);
		}
	}
}