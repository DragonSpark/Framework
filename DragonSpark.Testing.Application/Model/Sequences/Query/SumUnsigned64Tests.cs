using System.Linq;
using FluentAssertions;
using DragonSpark.Compose;
using DragonSpark.Testing.Objects;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Sequences.Query
{
	public sealed class SumUnsigned64Tests
	{
		const uint Total = 1000;

		readonly static string[] Source = Data.Default.Get().Take((int)Total).ToArray();

		[Fact]
		void Verify()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Array.By.Self.Query()
			     .Sum(x => (ulong)x.Length)
			     .Get(Source)
			     .Should()
			     .Be((ulong)Source.Sum(x => x.Length));
		}

		[Fact]
		void VerifySelect()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Array.By.Self.Query()
			     .Select(x => (ulong)x.Length)
			     .Sum()
			     .Get(Source)
			     .Should()
			     .Be((ulong)Source.Sum(x => x.Length));
		}
	}
}