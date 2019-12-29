using DragonSpark.Model.Results;
using System.Collections.Generic;

namespace DragonSpark.Testing.Objects
{
	sealed class AllNumbers : Instance<IEnumerable<int>>
	{
		public static IResult<IEnumerable<int>> Default { get; } = new AllNumbers();

		AllNumbers() : base(System.Linq.Enumerable.Range(0, int.MaxValue)) {}
	}
}