﻿using DragonSpark.Model.Results;
using Syncfusion.Blazor.Data;

namespace DragonSpark.Syncfusion.Queries
{
	public sealed class EmptyDataResult : Instance<DataResult>
	{
		public static EmptyDataResult Default { get; } = new();

		EmptyDataResult() : base(new DataResult()) {}
	}
}
