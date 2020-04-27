using DragonSpark.Compose;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Model.Sequences.Query
{
	// ReSharper disable once TestFileNameWarning
	public sealed class ConcatenationTests
	{
		const int skip = 10;

		readonly static int[] data =
		{
			0, 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8, 8, 8,
			8, 9, 9, 9, 9, 9, 9, 9, 9, 9
		};

		[Fact]
		public void Verify()
		{
			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Open.By.Self.Select(x => x.Append(1, 2, 3).ToArray())
			     .Get(data)
			     .Should()
			     .Equal(data.Concat(new[] {1, 2, 3}));
		}

		[Fact]
		public void VerifyBody()
		{
			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Open.By.Self.Select(x => x.Skip(skip).Append(1, 2, 3).ToArray())
			     .Get(data)
			     .Should()
			     .Equal(data.Skip(skip).Concat(new[] {1, 2, 3}));
		}

		[Fact]
		public void VerifyBodyFirst()
		{
			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Open.By.Self.Select(x => x.Skip(skip).Append(1, 2, 3).Skip(skip).FirstOrDefault())
			     .Get(data)
			     .Should()
			     .Be(data.Skip(skip).Concat(new[] {1, 2, 3}).Skip(skip).First());

			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Open.By.Self.Select(x => x.Skip(skip).Append(1, 2, 3).FirstOrDefault())
			     .Get(data)
			     .Should()
			     .Be(data.Skip(skip).Concat(new[] {1, 2, 3}).First());
		}
	}
}