using DragonSpark.Compose;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DragonSpark.Application.Compose.Entities.Generation;

public class PivotIncludeContext<T, TPivot, TCurrent> : PivotContext<T, TPivot>
	where T : class where TCurrent : class where TPivot : class
{
	readonly PivotGenerators<T, TPivot, TCurrent> _generators;
	readonly GeneratorState                       _state;

	public PivotIncludeContext(PivotGenerators<T, TPivot, TCurrent> generators, GeneratorState state)
		: base(generators.Subject, generators.Pivot, state)
	{
		_generators = generators;
		_state      = state;
	}

	public PivotContext<T, TCurrent> Pivot()
		=> new PivotContext<T, TCurrent>(_generators.Subject, _generators.Current, _state);

	public PivotIncludeContext<T, TPivot, TOther> ThenInclude<TOther>(Expression<Func<TCurrent, TOther>> property)
		where TOther : class
		=> ThenInclude(property, x => x);

	public PivotIncludeContext<T, TPivot, TOther> ThenInclude<TOther>(Expression<Func<TCurrent, TOther>> property,
	                                                                  Including<TCurrent, TOther> including)
		where TOther : class
	{
		var (subject, pivot, current) = _generators;
		var (generator, rule)         = _state.Rule(including);

		current.RuleFor(property, rule.Get);

		var generators = new PivotGenerators<T, TPivot, TOther>(subject, pivot, generator);
		var result     = new PivotIncludeContext<T, TPivot, TOther>(generators, _state);
		return result;
	}

	public PivotIncludeContext<T, TPivot, TOther> ThenInclude<TOther>(
		Expression<Func<TCurrent, ICollection<TOther>>> property)
		where TOther : class
		=> ThenInclude<TOther>(property, x => x);

	public PivotIncludeContext<T, TPivot, TOther> ThenInclude<TOther>(
		Expression<Func<TCurrent, ICollection<TOther>>> property,
		IncludingMany<TCurrent, TOther> including)
		where TOther : class
	{
		var (subject, pivot, current) = _generators;
		var (generator, rule)         = _state.RuleMany(including);

		current.RuleFor(property, rule.Get);

		var generators = new PivotGenerators<T, TPivot, TOther>(subject, pivot, generator);
		var result     = new PivotIncludeContext<T, TPivot, TOther>(generators, _state);
		return result;
	}

	public PivotIncludeContext<T, TPivot, TOther> ThenInclude<TOther>(Expression<Func<TCurrent, TOther>> property,
	                                                                  Expression<Func<TOther, TCurrent>> other)
		where TOther : class
		=> ThenInclude(property, other, x => x);

	public PivotIncludeContext<T, TPivot, TOther> ThenInclude<TOther>(Expression<Func<TCurrent, TOther>> property,
	                                                                  Expression<Func<TOther, TCurrent>> other,
	                                                                  Including<TCurrent, TOther> including)
		where TOther : class
	{
		var (subject, pivot, current) = _generators;
		var (generator, rule)         = _state.Rule(other, including);

		current.RuleFor(property, rule.Get);

		var generators = new PivotGenerators<T, TPivot, TOther>(subject, pivot, generator);
		var result     = new PivotIncludeContext<T, TPivot, TOther>(generators, _state);
		return result;
	}

	public PivotIncludeContext<T, TPivot, TOther> ThenInclude<TOther>(
		Expression<Func<TCurrent, ICollection<TOther>>> property,
		Expression<Func<TOther, TCurrent>> other)
		where TOther : class
		=> ThenInclude(property, other, x => x);

	public PivotIncludeContext<T, TPivot, TOther> ThenInclude<TOther>(
		Expression<Func<TCurrent, ICollection<TOther>>> property,
		Expression<Func<TOther, TCurrent>> other,
		IncludingMany<TCurrent, TOther> including)
		where TOther : class
	{
		var (subject, pivot, current) = _generators;
		var (generator, rule)         = _state.RuleMany(other, including);

		current.RuleFor(property, rule.Get);

		var generators = new PivotGenerators<T, TPivot, TOther>(subject, pivot, generator);
		var result     = new PivotIncludeContext<T, TPivot, TOther>(generators, _state);
		return result;
	}
}