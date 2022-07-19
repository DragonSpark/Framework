using DragonSpark.Runtime;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Runtime;

public sealed class EmitTests
{
	[Fact]
	public void Verify()
	{
		const string expected = "Hello World!";
		var          secure   = Secure.Default.Get(expected);
		var          emit     = Emit.Default.Get(secure);
		emit.Should().Be(expected);
	}
}