using DragonSpark.Compose;
using DragonSpark.Runtime.Environment;
using FluentAssertions;
using System;
using System.Reflection;
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
			     .Then()
			     .Accept()
			     .Ensure.Output.IsAssigned.Otherwise.Throw(PrimaryAssemblyMessage.Default)
			     .Get()
			     .Invoking(x => x.Get())
			     .Should()
			     .NotThrow();
		}

		[Fact]
		void VerifyGuard()
		{
			Start.A.Result<Assembly>()
			     .By.Calling(() => null)
			     .Then()
			     .Accept()
			     .Ensure.Output.IsAssigned.Otherwise.Throw(PrimaryAssemblyMessage.Default)
			     .Get()
			     .Invoking(x => x.Get())
			     .Should()
			     .Throw<InvalidOperationException>();
		}
	}
}