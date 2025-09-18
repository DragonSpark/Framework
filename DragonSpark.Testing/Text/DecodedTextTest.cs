using DragonSpark.Text;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace DragonSpark.Testing.Text;

[TestSubject(typeof(DecodedText))]
public class DecodedTextTest
{

	[Fact]
	public void Verify()
	{
		const string input    = "SGVsbG8gd29ybGQ="; // Base64 encoded string for "Hello world"
		const string expected = "Hello world";

		var result = DecodedText.Default.Get(input);
		result.Should().Be(expected);
	}
}