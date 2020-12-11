// ReSharper disable TooManyArguments

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

	public readonly struct Include<T, TOther> where TOther : class
	{
		readonly Func<Faker<TOther>, T, TOther> _generate;
		readonly Action<Faker, T, TOther>       _post;
		readonly AssignScope<T, TOther>         _scope;

		public Include(Func<Faker<TOther>, T, TOther> generate, Action<Faker, T, TOther> post,
		               AssignScope<T, TOther> scope)
		{
			_generate = generate;
			_post     = post;
			_scope    = scope;
		}

		public Include<T, TOther> Generate(Func<Faker<TOther>, T, TOther> generate)
			=> new Include<T, TOther>(generate, _post, _scope);

		public Include<T, TOther> Configure(Action<Faker, TOther> post)
			=> Configure((generator, _, instance) => post(generator, instance));

		public Include<T, TOther> Configure(Action<Faker, T, TOther> post)
			=> new Include<T, TOther>(_generate, post, _scope);

		public Include<T, TOther> Scoped(AssignScope<T, TOther> scope)
			=> new Include<T, TOther>(_generate, _post, scope);

		internal Payload Complete() => new Payload(_generate, _post, _scope);

		public readonly struct Payload
		{
			public Payload(Func<Faker<TOther>, T, TOther> generate, Action<Faker, T, TOther> post,
			               AssignScope<T, TOther> scope)

			{
				Generate = generate;
				Post     = post;
				Scope    = scope;
			}

			public Func<Faker<TOther>, T, TOther> Generate { get; }

			public Action<Faker, T, TOther> Post { get; }

			public AssignScope<T, TOther> Scope { get; }
		}
	}
}