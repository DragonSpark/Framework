using System.Collections.Generic;
using System.Linq;
using DragonSpark.Model.Results;

namespace DragonSpark.Testing.Objects
{
	sealed class AllNumbers : Instance<IEnumerable<int>>
	{
		public static IResult<IEnumerable<int>> Default { get; } = new AllNumbers();

		AllNumbers() : base(Enumerable.Range(0, int.MaxValue)) {}
	}
}