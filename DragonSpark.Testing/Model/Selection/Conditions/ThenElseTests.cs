using DragonSpark.Compose;
using DragonSpark.Compose.Model.Selection;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Model.Selection.Conditions
{
	public sealed class ThenElseTests
	{
		[Fact]
		public void Verify()
		{
			Subject.Default.Get(new Instance()).Should().BeFalse();
			Subject.Default.Get(new Instance{ Condition = true }).Should().BeTrue();
			Subject.Default.Get(new object()).Should().BeTrue();
			Subject.Default.Get(null!).Should().BeFalse();
		}

		sealed class Subject : Condition<object>
		{
			public static Subject Default { get; } = new Subject();

			Subject() : base(Is.Of<Instance>()
			                   .Then(Start.A.Selection<object>()
			                              .By.Cast<Instance>()
			                              .Select(x => x.Condition))
			                   .Else(Is.Assigned<object>())) {}
		}

		sealed class Instance
		{
			public bool Condition { get; set; }
		}
	}
}