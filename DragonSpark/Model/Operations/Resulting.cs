using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Resulting<T> : Result<ValueTask<T>>, IResulting<T>
	{
		public Resulting(IResult<ValueTask<T>> result) : base(result) {}

		public Resulting(Func<ValueTask<T>> source) : base(source) {}
	}
}