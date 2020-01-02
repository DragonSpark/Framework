using System.Linq;
using FluentAssertions;
using DragonSpark.Compose;
using DragonSpark.Testing.Objects;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Sequences.Query
{
	public sealed class Sum32Tests
	{
		const uint Total = 1000;

		readonly static string[] Source = Data.Default.Get().Take((int)Total).ToArray();

		[Fact]
		void Verify()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Query()
			     .Sum(x => x.Length)
			     .Get(Source)
			     .Should()
			     .Be(Source.Sum(x => x.Length));
		}

		[Fact]
		void VerifySelect()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Query()
			     .Select(x => x.Length)
			     .Sum()
			     .Get(Source)
			     .Should()
			     .Be(Source.Sum(x => x.Length));
		}
	}
}