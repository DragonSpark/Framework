using DragonSpark.Application.AspNet.Entities.Migration.Identity;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Application.Testing.Entities.Migration.Identity;

public sealed class StructuralEqualityComparerTests
{
	[Fact]
	public void Verify()
	{
		var                        key1 = new object[]{218, "AspNetCoreUserStore", "AuthenticatorKey"};
		var                        key2 = new object[]{218, "AspNetCoreUserStore", "AuthenticatorKey"};
		IEqualityComparer<object?> sut  = StructuralEqualityComparer.Default;
		sut.Equals(key1, key2).Should().BeTrue();
	}

	[Fact]
	public void VerifyDifferent()
	{
		var                        key1 = new object[]{218, "AspNetCoreUserStore", "AuthenticatorKey"};
		var                        key2 = new object[]{218u, "AspNetCoreUserStore", "AuthenticatorKey"};
		IEqualityComparer<object?> sut  = StructuralEqualityComparer.Default;
		sut.Equals(key1, key2).Should().BeTrue();
	}

}