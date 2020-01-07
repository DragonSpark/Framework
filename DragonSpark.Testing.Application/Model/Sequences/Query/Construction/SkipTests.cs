using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Query;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace DragonSpark.Testing.Application.Model.Sequences.Query.Construction
{
	public sealed class SkipTests
	{
		[Fact]
		void Verify()
		{
			var elements = new[]
			{
				0, 1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7, 8, 8, 8, 8, 8, 8,
				8, 8, 9, 9, 9, 9, 9, 9, 9, 9, 9
			};

			var selection = new Skip(10).Get(DragonSpark.Model.Sequences.Selection.Default);
			new ArrayView<int>(elements, selection.Start, (uint)elements.Length - selection.Start).ToArray()
			                                                                                      .Should()
			                                                                                      .Equal(elements
				                                                                                             .Skip(10));
		}
	}
}