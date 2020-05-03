using DragonSpark.Compose;
using DragonSpark.Runtime.Environment;
using FluentAssertions;
using System;
using System.Reflection;
using Xunit;

namespace DragonSpark.Testing.Runtime.Environment
{
	public sealed class PrimaryAssemblyMessageTests
	{
		[Fact]
		public void Verify()
		{
			Start.A.Result<Assembly>()
			     .By.Calling(() => null!)
			     .Invoking(x => x.Get())
			     .Should()
			     .NotThrow();
		}

		[Fact]
		public void VerifyAssigned()
		{
			Start.A.Result<Assembly>()
			     .By.Calling(() => GetType().Assembly)
			     .Accept()
			     .Ensure.Output.IsAssigned.Otherwise.Throw(PrimaryAssemblyMessage.Default)
			     .Get()
			     .Invoking(x => x.Get())
			     .Should()
			     .NotThrow();
		}

		[Fact]
		public void VerifyGuard()
		{
			Start.A.Result<Assembly>()
			     .By.Calling(() => null!)
			     .Accept()
			     .Ensure.Output.IsAssigned.Otherwise.Throw(PrimaryAssemblyMessage.Default)
			     .Get()
			     .Invoking(x => x.Get())
			     .Should()
			     .Throw<InvalidOperationException>();
		}
	}
}