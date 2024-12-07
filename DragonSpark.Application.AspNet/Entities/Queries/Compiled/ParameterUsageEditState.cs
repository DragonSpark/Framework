using DragonSpark.Compose;
using DragonSpark.Model.Sequences.Collections;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Entities.Queries.Compiled;

sealed class ParameterUsageEditState
{
	readonly IReadOnlyCollection<ParameterExpression> _parameters;
	readonly ParameterExpression                      _parameter;
	readonly IOrderedDictionary<string, Replacement>  _located;

	public ParameterUsageEditState(IReadOnlyCollection<ParameterExpression> parameters)
		: this(parameters, parameters.Last()) {}

	public ParameterUsageEditState(IReadOnlyCollection<ParameterExpression> parameters,
	                               ParameterExpression parameter)
		: this(parameters, parameter, new OrderedDictionary<string, Replacement>()) {}

	public ParameterUsageEditState(IReadOnlyCollection<ParameterExpression> parameters,
	                               ParameterExpression parameter,
	                               IOrderedDictionary<string, Replacement> located)
	{
		_parameters = parameters;
		_parameter  = parameter;
		_located    = located;
	}

	public bool Encountered { get; set; }

	bool Active { get; set; }

	bool Monitor { get; set; }

	public void Start()
	{
		if (Active)
		{
			throw new
				InvalidOperationException("An attempt to start was made, but this instance is already active.");
		}

		_located.Clear();
		Active      = true;
		Encountered = false;
		Monitor     = true;
	}

	public void Body()
	{
		Monitor = false;
	}

	public void Accept(ParameterExpression expression)
	{
		Encountered |= Monitor && expression == _parameter;
	}

	public Expression? Accept(MemberExpression expression)
	{
		if (Root.Default.Get(expression) == _parameter)
		{
			var key = expression.ToString();
			return _located.TryGetValue(key, out var existing) ? existing.Parameter : Add(key, expression);
		}

		return null;
	}

	ParameterExpression Add(string key, MemberExpression expression)
	{
		var result    = Expression.Parameter(expression.Type, $"{_parameter.Name}_parameter_{_located.Count}");
		var @delegate = Expression.Lambda(expression, _parameter.Yield()).Compile();
		_located.Add(key, new(expression.Type, @delegate, result));
		return result;
	}

	public RewriteResult Complete(LambdaExpression expression)
	{
		try
		{
			var start        = Encountered ? _parameters : _parameters.Except(_parameter.Yield());
			var replacements = _located.Values.AsValueEnumerable();
			var lambda = Expression.Lambda(expression.Body,
			                               start.Concat(replacements.Select(x => x.Parameter).ToArray()));
			var others = replacements.Select(x => x.Delegate).ToArray();
			var delegates = Encountered
				                ? others.Prepend(Expression.Lambda(_parameter, _parameter.Yield()).Compile())
				                        .ToArray()
				                : others;
			var types = Encountered
				            ? _located.Values.Select(x => x.ResultType).Prepend(_parameter.Type).ToArray()
				            : replacements.Select(x => x.ResultType).ToArray();
			return new RewriteResult(lambda, types, delegates);
		}
		finally
		{
			_located.Clear();
			Active = false;
		}
	}
}