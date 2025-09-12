using DragonSpark.Application.Runtime;
using FluentAssertions;
using JetBrains.Annotations;
using System;
using Xunit;

namespace DragonSpark.Application.Testing.Runtime;

[TestSubject(typeof(StandardUniqueIdentity))]
public class StandardUniqueIdentityTest
{

	[Fact]
	public void Verify()
	{
		var input    = new Guid("6caa12ab-f69c-47ef-142b-08ddf1d5a8e5");
		var expected = new Guid("6caa12ab-f69c-47ef-942b-08ddf1d5a8e5");
		var guid     = StandardUniqueIdentity.Default.Get(input);
		guid.Should().NotBe(input).And.Be(expected);
	}
}