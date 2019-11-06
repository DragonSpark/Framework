﻿using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Reflection;
using DragonSpark.Runtime.Execution;

namespace DragonSpark.Runtime.Objects
{
	sealed class OnlyOnceAlteration<TIn, TOut> : IAlteration<ISelect<TIn, TOut>>
	{
		public static OnlyOnceAlteration<TIn, TOut> Default { get; } = new OnlyOnceAlteration<TIn, TOut>();

		OnlyOnceAlteration() {}

		public ISelect<TIn, TOut> Get(ISelect<TIn, TOut> parameter)
			=> new First().Out()
			              .ToSelect(I.A<TIn>())
			              .To(parameter.If);
	}
}