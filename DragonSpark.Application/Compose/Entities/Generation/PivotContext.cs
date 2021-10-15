using Bogus;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DragonSpark.Application.Compose.Entities.Generation;

public class PivotContext<T, TCurrent> : IResult<T> where T : class where TCurrent : class
{
	readonly Faker<T>        _subject;
	readonly Faker<TCurrent> _current;
	readonly GeneratorState  _state;

	public PivotContext(Faker<T> subject, Faker<TCurrent> current, GeneratorState state)
	{
		_subject = subject;
		_current = current;
		_state   = state;
	}

	public PivotContext<T, TCurrent> Configure<TOther>(Expression<Func<TCurrent, TOther>> property,
	                                                   Func<Faker, TOther> configure)
		=> new PivotContext<T, TCurrent>(_subject, _current.RuleFor(property, configure), _state);

	public PivotContext<T, TCurrent> Configure(Alter<Faker<TCurrent>> configure)
		=> new PivotContext<T, TCurrent>(_subject, configure(_current), _state);

	public GeneratorContext<T> Return() => new GeneratorContext<T>(_subject, _state);

	public PivotIncludeContext<T, TCurrent, TOther> Include<TOther>(Expression<Func<TCurrent, TOther>> property)
		where TOther : class
		=> Include(property, x => x);

	public PivotIncludeContext<T, TCurrent, TOther> Include<TOther>(Expression<Func<TCurrent, TOther>> property,
	                                                                Including<TCurrent, TOther> including)
		where TOther : class
	{
		var (generator, rule) = _state.Rule(including);
		var configured = _current.RuleFor(property, rule.Get);
		var generators = new PivotGenerators<T, TCurrent, TOther>(_subject, configured, generator);
		var result     = new PivotIncludeContext<T, TCurrent, TOther>(generators, _state);
		return result;
	}

	public PivotIncludeContext<T, TCurrent, TOther> Include<TOther>(Expression<Func<TCurrent, TOther>> property,
	                                                                Expression<Func<TOther, TCurrent>> other)
		where TOther : class
		=> Include(property, other, x => x);

	public PivotIncludeContext<T, TCurrent, TOther> Include<TOther>(Expression<Func<TCurrent, TOther>> property,
	                                                                Expression<Func<TOther, TCurrent>> other,
	                                                                Including<TCurrent, TOther> including)
		where TOther : class
	{
		var (generator, rule) = _state.Rule(other, including);
		var configured = _current.RuleFor(property, rule.Get);
		var generators = new PivotGenerators<T, TCurrent, TOther>(_subject, configured, generator);
		var result     = new PivotIncludeContext<T, TCurrent, TOther>(generators, _state);
		return result;
	}

	public PivotIncludeContext<T, TCurrent, TOther> Include<TOther>(
		Expression<Func<TCurrent, ICollection<TOther>>> property)
		where TOther : class
		=> Include<TOther>(property, x => x);

	public PivotIncludeContext<T, TCurrent, TOther> Include<TOther>(
		Expression<Func<TCurrent, ICollection<TOther>>> property,
		IncludingMany<TCurrent, TOther> including)
		where TOther : class
	{
		var (generator, rule) = _state.RuleMany(including);
		var configured = _current.RuleFor(property, rule.Get);
		var generators = new PivotGenerators<T, TCurrent, TOther>(_subject, configured, generator);
		var result     = new PivotIncludeContext<T, TCurrent, TOther>(generators, _state);
		return result;
	}

	public PivotIncludeContext<T, TCurrent, TOther> Include<TOther>(
		Expression<Func<TCurrent, ICollection<TOther>>> property,
		Expression<Func<TOther, TCurrent>> other)
		where TOther : class
		=> Include(property, other, x => x);

	public PivotIncludeContext<T, TCurrent, TOther> Include<TOther>(
		Expression<Func<TCurrent, ICollection<TOther>>> property,
		Expression<Func<TOther, TCurrent>> other,
		IncludingMany<TCurrent, TOther> including)
		where TOther : class
	{
		var (generator, rule) = _state.RuleMany(other, including);
		var configured = _current.RuleFor(property, rule.Get);
		var generators = new PivotGenerators<T, TCurrent, TOther>(_subject, configured, generator);
		var result     = new PivotIncludeContext<T, TCurrent, TOther>(generators, _state);
		return result;
	}

	public T Get() => _subject.Generate();
}