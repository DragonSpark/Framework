using DragonSpark.Model.Results;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Testing.Objects
{
	sealed class AllNumbers : Instance<IEnumerable<int>>
	{
		public static IResult<IEnumerable<int>> Default { get; } = new AllNumbers();

		AllNumbers() : base(Enumerable.Range(0, int.MaxValue)) {}
	}
}