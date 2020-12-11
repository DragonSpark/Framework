// ReSharper disable TooManyArguments

using Bogus;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Application.Compose.Entities.Generation
{
	public class GeneratorContext<T> : IResult<T> where T : class
	{
		public static implicit operator T(GeneratorContext<T> instance) => instance.Get();

		readonly Faker<T>       _subject;
		readonly GeneratorState _store;

		public GeneratorContext() : this(new GeneratorState()) {}

		public GeneratorContext(GeneratorState store) : this(store.Get<T>(), store) {}

		public GeneratorContext(Faker<T> subject, GeneratorState store)
		{
			_subject = subject;
			_store   = store;
		}

		public IncludeGeneratorContext<T, TOther> Include<TOther>(Expression<Func<T, TOther>> property)
			where TOther : class
			=> Include(property, x => x);

		public IncludeGeneratorContext<T, TOther> Include<TOther>(Expression<Func<T, TOther>> property,
		                                                          Including<T, TOther> including)
			where TOther : class
		{
			var (generator, rule) = _store.Rule(including);
			var configured = _subject.RuleFor(property, rule.Get);
			var result     = new IncludeGeneratorContext<T, TOther>(configured, generator, _store);
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
			var (generator, rule) = _store.Rule(other, including);
			var configured = _subject.RuleFor(property, rule.Get);
			var result     = new IncludeGeneratorContext<T, TOther>(configured, generator, _store);
			return result;
		}

		public T Get() => _subject.Generate();
	}

	public class PivotContext<T, TCurrent> : IResult<T> where T : class where TCurrent : class
	{
		readonly Faker<T>        _subject;
		readonly Faker<TCurrent> _current;
		readonly GeneratorState  _store;

		public PivotContext(Faker<T> subject, Faker<TCurrent> current, GeneratorState store)
		{
			_subject = subject;
			_current = current;
			_store   = store;
		}

		public PivotIncludeContext<T, TCurrent, TOther> Include<TOther>(Expression<Func<TCurrent, TOther>> property)
			where TOther : class
			=> Include(property, x => x);

		public PivotIncludeContext<T, TCurrent, TOther> Include<TOther>(Expression<Func<TCurrent, TOther>> property,
		                                                                Including<TCurrent, TOther> including)
			where TOther : class
		{
			var (generator, rule) = _store.Rule(including);
			var configured = _current.RuleFor(property, rule.Get);
			var generators = new PivotGenerators<T, TCurrent, TOther>(_subject, configured, generator);
			var result     = new PivotIncludeContext<T, TCurrent, TOther>(generators, _store);
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
			var (generator, rule) = _store.Rule(other, including);
			var configured = _current.RuleFor(property, rule.Get);
			var generators = new PivotGenerators<T, TCurrent, TOther>(_subject, configured, generator);
			var result     = new PivotIncludeContext<T, TCurrent, TOther>(generators, _store);
			return result;
		}

		public T Get() => _subject.Generate();
	}

	public readonly struct PivotGenerators<T, TPivot, TCurrent>
		where T : class where TPivot : class where TCurrent : class
	{
		public PivotGenerators(Faker<T> subject, Faker<TPivot> pivot, Faker<TCurrent> current)
		{
			Subject = subject;
			Pivot   = pivot;
			Current = current;
		}

		public Faker<T> Subject { get; }

		public Faker<TPivot> Pivot { get; }

		public Faker<TCurrent> Current { get; }

		public void Deconstruct(out Faker<T> subject, out Faker<TPivot> pivot, out Faker<TCurrent> current)
		{
			subject = Subject;
			pivot   = Pivot;
			current = Current;
		}
	}

	public class PivotIncludeContext<T, TPivot, TCurrent> : PivotContext<T, TPivot>
		where T : class where TCurrent : class where TPivot : class
	{
		readonly PivotGenerators<T, TPivot, TCurrent> _generators;
		readonly GeneratorState                       _store;

		public PivotIncludeContext(PivotGenerators<T, TPivot, TCurrent> generators, GeneratorState store)
			: base(generators.Subject, generators.Pivot, store)
		{
			_generators = generators;
			_store      = store;
		}

		public PivotContext<T, TCurrent> Pivot()
			=> new PivotContext<T, TCurrent>(_generators.Subject, _generators.Current, _store);

		public PivotIncludeContext<T, TPivot, TOther> ThenInclude<TOther>(Expression<Func<TCurrent, TOther>> property)
			where TOther : class
			=> ThenInclude(property, x => x);

		public PivotIncludeContext<T, TPivot, TOther> ThenInclude<TOther>(Expression<Func<TCurrent, TOther>> property,
		                                                                  Including<TCurrent, TOther> including)
			where TOther : class
		{
			var (subject, pivot, current) = _generators;
			var (generator, rule)         = _store.Rule(including);

			current.RuleFor(property, rule.Get);

			var generators = new PivotGenerators<T, TPivot, TOther>(subject, pivot, generator);
			var result     = new PivotIncludeContext<T, TPivot, TOther>(generators, _store);
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
			var (generator, rule)         = _store.Rule(other, including);

			current.RuleFor(property, rule.Get);

			var generators = new PivotGenerators<T, TPivot, TOther>(subject, pivot, generator);
			var result     = new PivotIncludeContext<T, TPivot, TOther>(generators, _store);
			return result;
		}
	}

	sealed class GeneratorTables : ISelect<TypeInfo, IFakerTInternal>
	{
		public static GeneratorTables Default { get; } = new GeneratorTables();

		GeneratorTables() : this(new Generic<IResult<IFakerTInternal>>(typeof(Tables<>))) {}

		readonly IGeneric<IResult<IFakerTInternal>> _generic;

		public GeneratorTables(IGeneric<IResult<IFakerTInternal>> generic) => _generic = generic;

		public IFakerTInternal Get(TypeInfo parameter) => _generic.Get(parameter)().Get();

		sealed class Tables<T> : IResult<IFakerTInternal> where T : class
		{
			[UsedImplicitly]
			public static Tables<T> Instance { get; } = new Tables<T>();

			Tables() {}

			public IFakerTInternal Get() => Generator<T>.Default.Get();
		}
	}

	public sealed class GeneratorState
	{
		readonly ITypedTable<object>                _instances;
		readonly ISelect<TypeInfo, IFakerTInternal> _generators;

		public GeneratorState() : this(new TypedTable<object>(), GeneratorTables.Default.ToStandardTable()) {}

		public GeneratorState(ITypedTable<object> instances, ISelect<TypeInfo, IFakerTInternal> generators)
		{
			_instances  = instances;
			_generators = generators;
		}

		public Faker<T> Get<T>() where T : class => _generators.Get(A.Type<T>()).To<Faker<T>>();

		public (Faker<TOther>, IRule<T, TOther>) Rule<T, TOther>(Expression<Func<TOther, T>> other,
		                                                         Including<T, TOther> including)
			where TOther : class
		{
			var includes = including(Include.New<T, TOther>()).Complete();
			var configure = LocateAssignments<TOther, T>.Default.Get(other.GetMemberAccess().Name)
			                                            .Get()
			                                            .Verify($"The expression '{other}' did not resolve to a valid assignment setter.");
			var assign    = new Assign<T, TOther>(includes.Post, configure);
			var generator = _generators.Get(A.Type<TOther>()).To<Faker<TOther>>();
			var current   = new Rule<T, TOther>(generator, includes.Generate, assign.Execute);
			var result    = generator.Pair(includes.Scope(new Scope<T, TOther>(current, _instances)));
			return result;
		}

		public (Faker<TOther>, IRule<T, TOther>) Rule<T, TOther>(Including<T, TOther> including)
			where TOther : class
		{
			var includes   = including(Include.New<T, TOther>()).Complete();
			var assignment = LocateAssignment<TOther, T>.Default.Get();
			var configure = assignment != null
				                ? new Assign<T, TOther>(includes.Post, assignment).Execute
				                : includes.Post;
			var generator = _generators.Get(A.Type<TOther>()).To<Faker<TOther>>();
			var current   = new Rule<T, TOther>(generator, includes.Generate, configure);
			var result    = generator.Pair(includes.Scope(new Scope<T, TOther>(current, _instances)));
			return result;
		}
	}

	public class IncludeGeneratorContext<T, TCurrent> : GeneratorContext<T> where T : class where TCurrent : class
	{
		readonly Faker<T>        _subject;
		readonly Faker<TCurrent> _current;
		readonly GeneratorState  _store;

		public IncludeGeneratorContext(Faker<T> subject, Faker<TCurrent> current, GeneratorState store)
			: base(subject, store)
		{
			_subject = subject;
			_current = current;
			_store   = store;
		}

		public PivotContext<T, TCurrent> Pivot() => new PivotContext<T, TCurrent>(_subject, _current, _store);

		public IncludeGeneratorContext<T, TOther> ThenInclude<TOther>(Expression<Func<TCurrent, TOther>> property)
			where TOther : class
			=> ThenInclude(property, x => x);

		public IncludeGeneratorContext<T, TOther> ThenInclude<TOther>(Expression<Func<TCurrent, TOther>> property,
		                                                              Including<TCurrent, TOther> including)
			where TOther : class
		{
			var (generator, rule) = _store.Rule(including);

			_current.RuleFor(property, rule.Get);

			var result = new IncludeGeneratorContext<T, TOther>(_subject, generator, _store);
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
			var (generator, rule) = _store.Rule(other, including);

			_current.RuleFor(property, rule.Get);

			var result = new IncludeGeneratorContext<T, TOther>(_subject, generator, _store);
			return result;
		}
	}

	public static class Include
	{
		public static Include<T, TOther> New<T, TOther>() where TOther : class
			=> new Include<T, TOther>((generator, _) => generator.Generate(), (_, __, ___) => {},
			                          scope => scope.Once());
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

	public delegate Include<T, TOther> Including<T, TOther>(Include<T, TOther> include) where TOther : class;

	public delegate IRule<T, TOther> AssignScope<T, TOther>(Scope<T, TOther> scope) where TOther : class;

	public sealed class Scope<T, TOther> where TOther : class
	{
		readonly IRule<T, TOther>    _rule;
		readonly ITypedTable<object> _store;

		public Scope(IRule<T, TOther> rule, ITypedTable<object> store)
		{
			_rule  = rule;
			_store = store;
		}

		public IRule<T, TOther> PerCall() => _rule;

		public IRule<T, TOther> Once() => new StoredRule<T, TOther>(_rule, _store);
	}

	sealed class StoredRule<T, TOther> : IRule<T, TOther> where TOther : class
	{
		readonly IRule<T, TOther>    _previous;
		readonly ITypedTable<object> _store;
		readonly TypeInfo            _key;

		public StoredRule(IRule<T, TOther> previous, ITypedTable<object> store)
			: this(previous, store, A.Type<TOther>().GetTypeInfo()) {}

		public StoredRule(IRule<T, TOther> previous, ITypedTable<object> store, TypeInfo key)
		{
			_previous = previous;
			_store    = store;
			_key      = key;
		}

		public TOther Get((Faker, T) parameter)
		{
			if (!_store.TryGet(_key, out var existing))
			{
				var value = _previous.Get(parameter);
				_store.Assign(_key, value);
				return value;
			}

			var result = existing.To<TOther>();
			return result;
		}
	}
}