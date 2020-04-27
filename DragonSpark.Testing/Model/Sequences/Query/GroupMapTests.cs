using DragonSpark.Compose;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Model.Sequences.Query
{
	public sealed class GroupMapTests
	{
		readonly struct Entry
		{
			public Entry(string key, int value)
			{
				Key   = key;
				Value = value;
			}

			public string Key { get; }

			public int Value { get; }
		}

		[Fact]
		public void Verify()
		{
			var entries = new[]
			{
				new Entry("One", 1),
				new Entry("Two", 2),
				new Entry("Three", 3),

				new Entry("Four", 4),
				new Entry("Five", 5),
				new Entry("Two", 2)
			};

			var map = Start.A.Selection.Of.Type<Entry>()
			               .As.Sequence.Array.By.Self.Then()
			               .GroupMap(x => x.Key)
			               .Get(entries);

			map.Condition.Get("Two").Should().BeTrue();
			map.Condition.Get("Nine").Should().BeFalse();

			var array = map.Get("Two");
			array.Length.Should().Be(2);
			array.Open().Sum(x => x.Value).Should().Be(4);
		}
	}
}