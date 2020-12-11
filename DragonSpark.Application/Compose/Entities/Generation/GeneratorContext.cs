﻿// ReSharper disable TooManyArguments

using Bogus;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Compose.Entities.Generation
{
	public class GeneratorContext<T> : IResult<T> where T : class
	{
		public static implicit operator T(GeneratorContext<T> instance) => instance.Get();

		readonly Faker<T>       _subject;
		readonly GeneratorState _state;

		public GeneratorContext() : this(new GeneratorState()) {}

		public GeneratorContext(GeneratorState state) : this(state.Get<T>(), state) {}

		public GeneratorContext(Faker<T> subject, GeneratorState state)
		{
			_subject = subject;
			_state   = state;
		}

		public GeneratorContext<T> Configure<TOther>(Expression<Func<T, TOther>> property,
		                                             Func<Faker, TOther> configure)
			=> new GeneratorContext<T>(_subject.RuleFor(property, configure), _state);

		public IncludeGeneratorContext<T, TOther> Include<TOther>(Expression<Func<T, TOther>> property)
			where TOther : class
			=> Include(property, x => x);

		public IncludeGeneratorContext<T, TOther> Include<TOther>(Expression<Func<T, TOther>> property,
		                                                          Including<T, TOther> including)
			where TOther : class
		{
			var (generator, rule) = _state.Rule(including);
			var configured = _subject.RuleFor(property, rule.Get);
			var result     = new IncludeGeneratorContext<T, TOther>(configured, generator, _state);
			return result;
		}

		public IncludeGeneratorContext<T, TOther> Include<TOther>(Expression<Func<T, TOther>> property,
		                                                          Expression<Func<TOther, T>> other)
			where TOther : class
			=> Include(property, other, x => x);

		public IncludeGeneratorContext<T, TOther> Include<TOther>(Expression<Func<T, TOther>> property,
		                                                          Expression<Func<TOther, T>> other,
		                                                          Including<T, TOther> including)
			where TOther : class
		{
			var (generator, rule) = _state.Rule(other, including);
			var configured = _subject.RuleFor(property, rule.Get);
			var result     = new IncludeGeneratorContext<T, TOther>(configured, generator, _state);
			return result;
		}

		public T Get() => _subject.Generate();
	}

}