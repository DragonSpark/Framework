using DragonSpark.Runtime;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace DragonSpark.Testing.Runtime;

public sealed class VersionFormatterTests
{
    [Fact]
    public void Verify()
    {
        VersionFormatter.Default.Get(IdentitySchemaVersions.Version3).Should().Be("3.0.0");
        
    }
}