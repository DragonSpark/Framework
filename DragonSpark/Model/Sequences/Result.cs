using System.Collections.Generic;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences
{
	sealed class Result<T> : Select<IEnumerable<T>, Array<T>>
	{
		public static Result<T> Default { get; } = new Result<T>();

		Result() : base(x => x.Open()) {}
	}
}