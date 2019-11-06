﻿using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime.Objects
{
	sealed class ResultAwareCast<TFrom, TTo> : Select<TFrom, TTo>
	{
		public static ResultAwareCast<TFrom, TTo> Default { get; } = new ResultAwareCast<TFrom, TTo>();

		ResultAwareCast() : base(Start.A.Selection<TFrom>()
		                              .AndOf<TTo>()
		                              .By.Cast.Unless(CanCast<TFrom, IResult<TTo>>.Default,
		                                              CastOrThrow<TFrom, IResult<TTo>>
			                                              .Default.Then()
			                                              .Value()
			                                              .Get())) {}
	}
}