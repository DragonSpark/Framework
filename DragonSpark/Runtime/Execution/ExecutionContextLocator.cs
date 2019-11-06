﻿using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Reflection;
using DragonSpark.Runtime.Environment;

namespace DragonSpark.Runtime.Execution
{
	sealed class ExecutionContextLocator : Result<IExecutionContext>
	{
		public static ExecutionContextLocator Default { get; } = new ExecutionContextLocator();

		ExecutionContextLocator() : base(A.This(ComponentTypesDefinition.Default)
		                                  .Select(x => x.Query().FirstAssigned())
		                                  .Assume()
		                                  .To(I<ComponentLocator<IExecutionContext>>.Default)) {}
	}
}