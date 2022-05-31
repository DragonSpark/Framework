using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using System.Linq.Expressions;

namespace DragonSpark.Runtime.Invocation.Expressions;

class Delegates<TIn, TOut> : ReferenceValueTable<TIn, TOut> where TIn : class where TOut : class
{
	protected Delegates(ISelect<TIn, Expression> select) : this(select, Empty<ParameterExpression>.Array) {}

	protected Delegates(ISelect<TIn, Expression> select, params ParameterExpression[] parameters)
		: base(select.Then().Select(new Lambda<TOut>(parameters)).Compile()) {}
}