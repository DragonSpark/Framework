using DragonSpark.Model.Results;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Allocated : Result<Task>, IAllocated
	{
		public Allocated(IResult<Task> result) : base(result) {}

		public Allocated(Func<Task> source) : base(source) {}
	}
}