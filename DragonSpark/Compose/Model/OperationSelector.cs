﻿using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using System.Threading.Tasks;

namespace DragonSpark.Compose.Model
{
	public class OperationSelector : ResultContext<ValueTask>
	{
		public static implicit operator Operate(OperationSelector instance) => instance.Get().Get;

		public static implicit operator Await(OperationSelector instance) => instance.Get().Await;

		public OperationSelector(IResult<ValueTask> instance) : base(instance) {}

		public OperationSelector Append(Await next) => new OperationSelector(new Appended(Get().Await, next));

		public OperationSelector Append(Operate next) => new OperationSelector(new AppendedOperate(Get().Get, next));

		public AllocatedOperationSelector Allocate() => new AllocatedOperationSelector(Select(x => x.AsTask()).Get());
	}
}