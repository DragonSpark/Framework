﻿using System;

namespace DragonSpark.Model.Selection.Stores
{
	public class Store<TIn, TOut> : Select<TIn, TOut>
	{
		protected Store(Func<TIn, TOut> source) : base(Stores<TIn, TOut>.Default.Get(source)) {}
	}
}