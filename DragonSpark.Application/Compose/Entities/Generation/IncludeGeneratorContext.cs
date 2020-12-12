using Bogus;
using DragonSpark.Compose;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DragonSpark.Application.Compose.Entities.Generation
{
	public class IncludeGeneratorContext<T, TCurrent> : GeneratorContext<T> where T : class where TCurrent : class
	{
		readonly Faker<T>        _subject;
		readonly Faker<TCurrent> _current;
		readonly GeneratorState  _state;

		public IncludeGeneratorContext(Faker<T> subject, Faker<TCurrent> current, GeneratorState state)
			: base(subject, state)
		{
			_subject = subject;
			_current = current;
			_state   = state;
		}

		public PivotContext<T, TCurrent> Pivot() => new PivotContext<T, TCurrent>(_subject, _current, _state);

		public IncludeGeneratorContext<T, TOther> ThenInclude<TOther>(Expression<Func<TCurrent, TOther>> property)
			where TOther : class
			=> ThenInclude(property, x => x);

		public IncludeGeneratorContext<T, TOther> ThenInclude<TOther>(Expression<Func<TCurrent, TOther>> property,
		                                                              Including<TCurrent, TOther> including)
			where TOther : class
		{
			var (generator, rule) = _state.Rule(including);

			_current.RuleFor(property, rule.Get);

			var result = new IncludeGeneratorContext<T, TOther>(_subject, generator, _state);
			return result;
		}

		public IncludeGeneratorContext<T, TOther> ThenInclude<TOther>(
			Expression<Func<TCurrent, ICollection<TOther>>> property)
			where TOther : class
			=> ThenInclude<TOther>(property, x => x);

		public IncludeGeneratorContext<T, TOther> ThenInclude<TOther>(
			Expression<Func<TCurrent, ICollection<TOther>>> property,
			IncludingMany<TCurrent, TOther> including)
			where TOther : class
		{
			var (generator, rule) = _state.RuleMany(including);

			_current.RuleFor(property, rule.Get);

			var result = new IncludeGeneratorContext<T, TOther>(_subject, generator, _state);
			return result;
		}

		public IncludeGeneratorContext<T, TOther> ThenInclude<TOther>(Expression<Func<TCurrent, TOther>> property,
		                                                              Expression<Func<TOther, TCurrent>> other)
			where TOther : class
			=> ThenInclude(property, other, x => x);

		public IncludeGeneratorContext<T, TOther> ThenInclude<TOther>(Expression<Func<TCurrent, TOther>> property,
		                                                              Expression<Func<TOther, TCurrent>> other,
		                                                              Including<TCurrent, TOther> including)
			where TOther : class
		{
			var (generator, rule) = _state.Rule(other, including);

			_current.RuleFor(property, rule.Get);

			var result = new IncludeGeneratorContext<T, TOther>(_subject, generator, _state);
			return result;
		}

		public IncludeGeneratorContext<T, TOther> ThenInclude<TOther>(
			Expression<Func<TCurrent, ICollection<TOther>>> property,
			Expression<Func<TOther, TCurrent>> other)
			where TOther : class
			=> ThenInclude(property, other, x => x);

		public IncludeGeneratorContext<T, TOther> ThenInclude<TOther>(
			Expression<Func<TCurrent, ICollection<TOther>>> property,
			Expression<Func<TOther, TCurrent>> other,
			IncludingMany<TCurrent, TOther> including)
			where TOther : class
		{
			var (generator, rule) = _state.RuleMany(other, including);

			_current.RuleFor(property, rule.Get);

			var result = new IncludeGeneratorContext<T, TOther>(_subject, generator, _state);
			return result;
		}
	}
}