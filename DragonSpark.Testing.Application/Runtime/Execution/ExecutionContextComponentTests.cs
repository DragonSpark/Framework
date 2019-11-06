using System.Linq;
using FluentAssertions;
using JetBrains.Annotations;
using DragonSpark.Application.Hosting.xUnit;
using DragonSpark.Model.Results;
using DragonSpark.Runtime.Environment;
using DragonSpark.Runtime.Execution;
using Xunit;

namespace DragonSpark.Testing.Application.Runtime.Execution
{
	[TestCaseOrderer("DragonSpark.Application.Hosting.xUnit.PriorityOrderer", "DragonSpark.Application.Hosting.xUnit")]
	public sealed class ExecutionContextComponentTests
	{
		sealed class DefaultExecutionContext : Result<object>, IExecutionContext
		{
			[UsedImplicitly]
			public static IExecutionContext Default { get; } = new DefaultExecutionContext();

			DefaultExecutionContext() : base(() => new ContextDetails("Local Context")) {}
		}

		[Fact, TestPriority(2)]
		void Empty()
		{
			Types.Default.Execute(typeof(object).Yield().ToArray());

			ExecutionContext.Default.Get()
			                .To<ContextDetails>()
			                .Details.Name.Should()
			                .Be("Default Execution Context");
		}

		[Fact, TestPriority(0)]
		void Override()
		{
			Types.Default.Execute(typeof(DefaultExecutionContext).Yield().ToArray());

			ExecutionContext.Default.Get()
			                .To<ContextDetails>()
			                .Details.Name.Should()
			                .Be("Local Context");
		}

		[Fact, TestPriority(1)]
		void Verify()
		{
			ExecutionContext.Default.Get()
			                .To<ContextDetails>()
			                .Details.Name.Should()
			                .Be("xUnit Testing Application Default (root) Execution Context");
		}

		[Fact]
		void VerifyTest()
		{
			ExecutionContext.Default.Get().Should().BeSameAs(ExecutionContext.Default.Get());
		}
	}
}