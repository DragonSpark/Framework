using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Sequences.Query
{
	sealed class InlineSelections<TFrom, TTo> : ISelect<Expression<Func<TFrom, TTo>>, Expression<Copy<TFrom, TTo>>>
	{
		public static InlineSelections<TFrom, TTo> Default { get; } = new InlineSelections<TFrom, TTo>();

		InlineSelections() : this(Expression.Parameter(typeof(TFrom[]), "source"),
		                          Expression.Parameter(typeof(TTo[]), "destination"),
		                          Expression.Parameter(typeof(uint), "start"),
		                          Expression.Parameter(typeof(uint), "finish"),
		                          Expression.Parameter(typeof(uint), "to")) {}

		readonly IEnumerable<ParameterExpression>     _input;
		readonly ISelect<string, ParameterExpression> _parameters;

		public InlineSelections(params ParameterExpression[] parameters)
			: this(parameters, parameters.ToDictionary(x => x.Name).ToTable()) {}

		public InlineSelections(IEnumerable<ParameterExpression> input, ISelect<string, ParameterExpression> parameters)
		{
			_input      = input;
			_parameters = parameters;
		}

		public Expression<Copy<TFrom, TTo>> Get(Expression<Func<TFrom, TTo>> parameter)
		{
			var from = _parameters.Get("start");
			var @in  = Expression.ArrayAccess(_parameters.Get("source"), Expression.Convert(from, typeof(int)));
			var to   = _parameters.Get("to");
			var @out = Expression.ArrayAccess(_parameters.Get("destination"), Expression.Convert(to, typeof(int)));

			var inline = new InlineVisitor(parameter.Parameters[0], @in).Visit(parameter.Body)
			             ??
			             throw new InvalidOperationException("Inline expression was not found");

			var label = Expression.Label();
			var body = Expression.Block(Expression.PostDecrementAssign(from),
			                            Expression.Loop(Expression
				                                            .IfThenElse(Expression
					                                                        .LessThan(Expression.PreIncrementAssign(from),
					                                                                  _parameters.Get("finish")),
				                                                        Expression
					                                                        .Block(Expression.Assign(@out, inline),
					                                                               Expression.PostIncrementAssign(to)
					                                                              ),
				                                                        Expression.Break(label)),
			                                            label));

			var result = Expression.Lambda<Copy<TFrom, TTo>>(body, _input);
			return result;
		}
	}
}