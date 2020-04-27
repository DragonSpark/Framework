using DragonSpark.Model.Sequences.Collections;
using FluentAssertions;
using Xunit;

namespace DragonSpark.Testing.Model.Sequences.Collections
{
	public sealed class SortSelectorTests
	{
		sealed class Subject {}

		sealed class Aware : ISortAware
		{
			public int Get() => 6776;
		}

		[Sort(123)]
		sealed class Declared {}

		[Fact]
		public void Verify()
		{
			SortSelector<Subject>.Default.Get(new Subject()).Should().Be(-1);
		}

		[Fact]
		public void VerifyAware()
		{
			SortSelector<Aware>.Default.Get(new Aware()).Should().Be(6776);
		}

		[Fact]
		public void VerifyDeclared()
		{
			SortSelector<Declared>.Default.Get(new Declared()).Should().Be(123);
		}
	}
}