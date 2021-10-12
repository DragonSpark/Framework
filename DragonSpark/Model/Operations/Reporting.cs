﻿using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class Reporting<TIn, TOut> : ISelecting<TIn, TOut>
	{
		readonly ISelecting<TIn, TOut> _previous;
		readonly Action<Task<TOut>>    _report;

		public Reporting(ISelecting<TIn, TOut> previous, Action<Task<TOut>> report)
		{
			_previous = previous;
			_report   = report;
		}

		public ValueTask<TOut> Get(TIn parameter)
		{
			var result = _previous.Get(parameter);
			_report.Invoke(result.AsTask());
			return result;
		}
	}
}