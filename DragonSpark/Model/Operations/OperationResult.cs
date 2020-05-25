﻿using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Selecting<TIn, TOut> : Select<TIn, ValueTask<TOut>>, ISelecting<TIn, TOut>
	{
		public Selecting(ISelect<TIn, ValueTask<TOut>> select) : this(select.Get) {}

		public Selecting(Func<TIn, ValueTask<TOut>> select) : base(select) {}
	}

	public class OperationResult<T> : Result<ValueTask<T>>, IResulting<T>
	{
		public OperationResult(IResult<ValueTask<T>> result) : base(result) {}

		public OperationResult(Func<ValueTask<T>> source) : base(source) {}
	}
}