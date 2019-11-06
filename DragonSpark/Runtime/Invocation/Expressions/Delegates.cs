﻿using System.Linq.Expressions;
using DragonSpark.Model.Selection;

namespace DragonSpark.Runtime.Invocation.Expressions
{
	class Delegates<TIn, TOut> : Select<TIn, TOut>
	{
		public Delegates(ISelect<TIn, Expression> select) : this(select, Empty<ParameterExpression>.Array) {}

		protected Delegates(ISelect<TIn, Expression> select, params ParameterExpression[] parameters)
			: base(select.Then().Select(new Lambda<TOut>(parameters)).Compile()) {}
	}
}