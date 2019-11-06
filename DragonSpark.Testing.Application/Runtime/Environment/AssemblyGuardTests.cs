using System;
using System.Reflection;
using FluentAssertions;
using DragonSpark.Compose;
using DragonSpark.Runtime.Environment;
using Xunit;

namespace DragonSpark.Testing.Application.Runtime.Environment
{
	public sealed class AssemblyGuardTests
	{
		[Fact]
		void Verify()
		{
			Start.A.Result<Assembly>()
			     .By.Calling(() => null)
			     .Invoking(x => x.Get())
			     .Should()
			     .NotThrow();
		}

		[Fact]
		void VerifyAssigned()
		{
			Start.A.Result<Assembly>()
			     .By.Calling(() => GetType().Assembly)
			     .ToSelect()
			     .Select(PrimaryAssemblyMessage.Default.AsGuard())
			     .Invoking(x => x.Get())
			     .Should()
			     .NotThrow();
		}

		[Fact]
		void VerifyGuard()
		{
			Start.A.Result<Assembly>()
			     .By.Calling(() => null)
			     .ToSelect()
			     .Select(PrimaryAssemblyMessage.Default.AsGuard())
			     .Invoking(x => x.Get())
			     .Should()
			     .Throw<InvalidOperationException>();
		}
	}
}