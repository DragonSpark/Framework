using DragonSpark.Compose;
using DragonSpark.Testing.Objects;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Sequences.Query
{
	public sealed class Sum64Tests
	{
		const uint Total = 1000;

		readonly static string[] Source = Data.Default.Get().Take((int)Total).ToArray();

		[Fact]
		void Verify()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Query()
			     .Reduce(x => x.Sum(y => (long)y.Length))
			     .Get(Source)
			     .Should()
			     .Be(Source.Sum(x => (long)x.Length));
		}

		[Fact]
		void VerifySelect()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Open.By.Self.Query()
			     .Select(x => (long)x.Length)
			     .Reduce(x => x.Sum())
			     .Get(Source)
			     .Should()
			     .Be(Source.Sum(x => (long)x.Length));
		}
	}
}