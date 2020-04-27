using DragonSpark.Model.Selection;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Reflection.Types
{
	public sealed class ActivationDelegatesTests
	{
		sealed class Counter : ISelect<object, int>
		{
			public static Counter Default { get; } = new Counter();

			Counter() {}

			int _count;

			public int Get(object parameter) => _count++;
		}

		[Fact]
		public void Verify()
		{
			var first = Delegates<object, int>.Default.Get(Counter.Default);
			first.Should().BeSameAs(Delegates<object, int>.Default.Get(Counter.Default));

			var o = new object();
			first(o);
		}
	}
}