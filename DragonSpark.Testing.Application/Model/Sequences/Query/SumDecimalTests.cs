using System.Linq;
using FluentAssertions;
using DragonSpark.Compose;
using DragonSpark.Testing.Objects;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Sequences.Query
{
	public sealed class SumDecimalTests
	{
		const uint Total = 1000;

		readonly static string[] Source = Data.Default.Get().Take((int)Total).ToArray();

		[Fact]
		void Verify()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Array.By.Self.Query()
			     .Sum(x => (decimal)x.Length)
			     .Get(Source)
			     .Should()
			     .Be(Source.Sum(x => (decimal)x.Length));
		}

		[Fact]
		void VerifySelect()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Array.By.Self.Query()
			     .Select(x => (decimal)x.Length)
			     .Sum()
			     .Get(Source)
			     .Should()
			     .Be(Source.Sum(x => (decimal)x.Length));
		}
	}
}