using DragonSpark.Compose;
using DragonSpark.Runtime.Execution;
using DragonSpark.Testing.Objects;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Application.Runtime.Execution
{
	public sealed class ContextualTests
	{
		sealed class Resource : Contextual<CountingDisposable>
		{
			public static Resource Default { get; } = new Resource();

			Resource() : base(A.New<CountingDisposable>()) {}
		}

		[Fact]
		void VerifyResource()
		{
			ExecutionContextStore.Default.Condition.Get().Should().BeFalse();
			var instance = Resource.Default.Get();
			var context  = ExecutionContextStore.Default.Get();
			context.Should().NotBeNull();
			AssociatedResources.Default.Condition.Get(context).Should().BeTrue();
			instance.Get().Should().Be(0);
			ExecutionContextStore.Default.Condition.Get().Should().BeTrue();
			DisposeContext.Default.Execute();

			instance.Get().Should().Be(1);
			ExecutionContextStore.Default.Condition.Get().Should().BeFalse();
			AssociatedResources.Default.Condition.Get(context).Should().BeFalse();

			DisposeContext.Default.Execute();
			instance.Get().Should().Be(1);
			ExecutionContextStore.Default.Condition.Get().Should().BeFalse();
			AssociatedResources.Default.Condition.Get(context).Should().BeFalse();
		}
	}
}