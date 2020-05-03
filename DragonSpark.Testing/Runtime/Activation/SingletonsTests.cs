using DragonSpark.Model.Results;
using DragonSpark.Runtime.Activation;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace DragonSpark.Testing.Runtime.Activation
{
	public class SingletonsTests
	{
		sealed class Subject : IResult<int>
		{
			public static Subject Default { get; } = new Subject();

			Subject() {}

			public int Get() => 6776;
		}

		sealed class Not
		{
			[UsedImplicitly]
			public object InvalidName { get; set; } = default!;
		}

		[Fact]
		public void Is() => Singletons.Default.Get(typeof(Subject))
		                              .Should()
		                              .BeSameAs(Subject.Default);

		[Fact]
		public void IsNot() => Singletons.Default.Get(typeof(Not))
		                                 .Should()
		                                 .BeNull();
	}
}