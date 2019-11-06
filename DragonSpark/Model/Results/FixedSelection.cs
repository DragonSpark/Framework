﻿using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Results
{
	public class FixedSelection<TIn, TOut> : IResult<TOut>
	{
		readonly TIn             _parameter;
		readonly Func<TIn, TOut> _source;

		public FixedSelection(ISelect<TIn, TOut> select, TIn parameter) : this(select.Get, parameter) {}

		public FixedSelection(Func<TIn, TOut> source, TIn parameter)
		{
			_source    = source;
			_parameter = parameter;
		}

		public TOut Get() => _source(_parameter);
	}
}