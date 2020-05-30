using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.DependencyInjection;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;

namespace DragonSpark.Composition.Compose
{
	public static class Extensions
	{
		public static StartRegistration<T> Start<T>(this IServiceCollection @this) where T : class
			=> new StartRegistration<T>(@this);

		public static IRegistration Then(this IRegistration @this, IRegistration next)
			=> new LinkedRegistrationContext(@this, next);

		public static IExpander Then(this IExpander @this, IRegistration context) => @this.Then(context.Fixed());

		public static IExpander Then(this IExpander @this, IExpander next) => new LinkedExpander(@this, next);

		public static IExpander Fixed(this IRegistration @this) => new FixedExpander(@this);

		public static RegistrationResult Result(this IServiceCollection @this) => new RegistrationResult(@this);

		public static IRelatedTypes Recursive(this Dependencies _) => RecursiveDependencies.Default;
	}

	public readonly struct RegistrationResult
	{
		public RegistrationResult(IServiceCollection then) => Then = then;

		public IServiceCollection Then { get; }
	}

	public interface IRegistrationWithInclude : IRegistration
	{
		IRegistration Include(IRelatedTypes related);
	}

	public interface IRegistration
	{
		RegistrationResult Singleton();

		RegistrationResult Transient();

		RegistrationResult Scoped();
	}

	public sealed class StartRegistration<T> : IRegistrationWithInclude where T : class
	{
		readonly IServiceCollection _subject;

		public StartRegistration(IServiceCollection subject) => _subject = subject;

		IExpander Current => new Register<T>(_subject).Fixed();

		public Registrations And<TNext>() where TNext : class
			=> new Registrations(_subject,
			                     Current.Then(new TypeExpander<TNext>(_subject)
				                                  .Then(new Register<TNext>(_subject))));

		public Registration<T> Forward<TTo>() where TTo : class, T
			=> new Registration<T>(_subject,
			                       new TypeExpander<TTo>(_subject).Then(new Forward<T, TTo>(_subject)));

		public IRegistration Include(Func<RelatedTypesHolster, IRelatedTypes> related)
			=> Include(related(RelatedTypesHolster.Default));

		public IRegistration Include(IRelatedTypes related) => Current.Get(related.Get(_subject));

		public RegistrationResult Singleton() => Include(x => x.AsIs).Singleton();

		public RegistrationResult Transient() => Include(x => x.AsIs).Transient();

		public RegistrationResult Scoped() => Include(x => x.AsIs).Scoped();
	}

	public class RegistrationWithInclude : IRegistrationWithInclude
	{
		readonly IExpander _current;

		public RegistrationWithInclude(IServiceCollection services, IRegistration next)
			: this(services, next.Fixed()) {}

		public RegistrationWithInclude(IServiceCollection services, IExpander current)
		{
			_current = current;
			Services = services;
		}

		protected IServiceCollection Services { get; }

		protected IExpander Next(IRegistration next) => Next(next.Fixed());

		protected IExpander Next(IExpander next) => _current.Then(next);

		public IRegistration Include(Func<RelatedTypesHolster, IRelatedTypes> related)
			=> Include(related(RelatedTypesHolster.Default));

		public IRegistration Include(IRelatedTypes related) => _current.Get(related.Get(Services));

		public RegistrationResult Singleton() => Include(x => x.AsIs).Singleton();

		public RegistrationResult Transient() => Include(x => x.AsIs).Transient();

		public RegistrationResult Scoped() => Include(x => x.AsIs).Scoped();
	}

	public sealed class Registrations : RegistrationWithInclude
	{
		public Registrations(IServiceCollection services, IExpander current) : base(services, current) {}

		public Registrations And<TNext>() where TNext : class
			=> new Registrations(Services, Next(new TypeExpander<TNext>(Services).Then(new Register<TNext>(Services))));
	}

	public sealed class RelatedTypesHolster
	{
		public static RelatedTypesHolster Default { get; } = new RelatedTypesHolster();

		RelatedTypesHolster() : this(RelatedTypes.Default, Dependencies.Default) {}

		public RelatedTypesHolster(IRelatedTypes asIs, Dependencies dependencies)
		{
			AsIs         = asIs;
			Dependencies = dependencies;
		}

		public IRelatedTypes AsIs { get; }

		public Dependencies Dependencies { get; }
	}

	public interface IRelatedTypes : ISelect<IServiceCollection, IIncludes> {}

	public sealed class Dependencies : IRelatedTypes
	{
		public static Dependencies Default { get; } = new Dependencies();

		Dependencies() {}

		public IIncludes Get(IServiceCollection parameter) => new DependencyIncludes(parameter);
	}

	sealed class RecursiveDependencies : IRelatedTypes
	{
		public static RecursiveDependencies Default { get; } = new RecursiveDependencies();

		RecursiveDependencies() {}

		public IIncludes Get(IServiceCollection parameter)
			=> new RecursiveDependencyIncludes(new IncludeTracker(parameter));
	}

	sealed class RelatedTypes : FixedResult<IServiceCollection, IIncludes>, IRelatedTypes
	{
		public static RelatedTypes Default { get; } = new RelatedTypes();

		RelatedTypes() : this(Includes.Default) {}

		public RelatedTypes(IIncludes instance) : base(instance) {}
	}

	public interface IIncludes : IArray<Type, Type> {}

	sealed class RecursiveDependencyIncludes : IIncludes
	{
		readonly IIncludeTracker _includes;

		public RecursiveDependencyIncludes(IIncludeTracker includes) => _includes = includes;

		public Array<Type> Get(Type parameter)
		{
			/*
			var history = new HashSet<Type>();
			var result = _includes.Get(history, parameter)
			                      .Aggregate(System.Linq.Enumerable.Empty<Type>(),
			                                 (current, type) => current.Union(_includes.Get(history, type)))
			                      .ToArray();
			return result;*/
			var result = Yield(new HashSet<Type>(), parameter).AsValueEnumerable().Distinct().ToArray();
			return result;
		}

		IEnumerable<Type> Yield(ISet<Type> history, Type current)
		{
			foreach (var type in _includes.Get(history, current))
			{
				yield return type;

				foreach (var other in Yield(history, type))
				{
					yield return other;
				}
			}
		}
	}

	public interface IIncludeTracker : ISelect<(ISet<Type> History, Type Current), IEnumerable<Type>> {}

	sealed class IncludeTracker : IIncludeTracker
	{
		readonly Func<Type, bool>   _can;
		readonly IArray<Type, Type> _candidates;

		public IncludeTracker(IServiceCollection services)
			: this(new CanRegister(services).Get, DependencyCandidates.Default) {}

		public IncludeTracker(Func<Type, bool> can, IArray<Type, Type> candidates)
		{
			_can        = can;
			_candidates = candidates;
		}

		public IEnumerable<Type> Get((ISet<Type> History, Type Current) parameter)
		{
			var (history, current) = parameter;
			foreach (var candidate in _candidates.Get(current).Open())
			{
				var add = history.Add(candidate);
				var can = _can(candidate);
				if (add && can)
				{
					yield return candidate;
				}
			}
		}
	}

	sealed class DependencyIncludes : IIncludes
	{
		readonly IIncludeTracker _tracker;

		public DependencyIncludes(IServiceCollection services) : this(new IncludeTracker(services)) {}

		public DependencyIncludes(IIncludeTracker tracker) => _tracker = tracker;

		public Array<Type> Get(Type parameter)
			=> _tracker.Get(new HashSet<Type>(), parameter).AsValueEnumerable().ToArray();
	}

	sealed class Includes : Select<Type, Array<Type>>, IIncludes
	{
		public static Includes Default { get; } = new Includes();

		Includes() : base(x => x.Yield().Result()) {}
	}

	sealed class LinkedRegistrationContext : IRegistration
	{
		readonly IRegistration _next, _previous;

		public LinkedRegistrationContext(IRegistration previous, IRegistration next)
		{
			_previous = previous;
			_next     = next;
		}

		public RegistrationResult Singleton()
		{
			_previous.Singleton();
			var result = _next.Singleton();
			return result;
		}

		public RegistrationResult Transient()
		{
			_previous.Transient();
			var result = _next.Transient();
			return result;
		}

		public RegistrationResult Scoped()
		{
			_previous.Scoped();
			var result = _next.Scoped();
			return result;
		}
	}

	sealed class TypesRegistration : IRegistration
	{
		readonly IServiceCollection _services;
		readonly Array<Type>        _types;

		public TypesRegistration(IServiceCollection services, Array<Type> types)
		{
			_services = services;
			_types    = types;
		}

		public RegistrationResult Singleton()
		{
			var services = _services;
			var length   = _types.Length;
			for (var i = 0; i < length; i++)
			{
				services = services.AddSingleton(_types[i]);
			}

			var result = services.Result();
			return result;
		}

		public RegistrationResult Transient()
		{
			var services = _services;
			var length   = _types.Length;
			for (var i = 0; i < length; i++)
			{
				services = services.AddTransient(_types[i]);
			}

			var result = services.Result();
			return result;
		}

		public RegistrationResult Scoped()
		{
			var services = _services;
			var length   = _types.Length;
			for (var i = 0; i < length; i++)
			{
				services = services.AddScoped(_types[i]);
			}

			var result = services.Result();
			return result;
		}
	}

	public interface IExpander : ISelect<IIncludes, IRegistration> {}

	sealed class LinkedExpander : IExpander
	{
		readonly IExpander _first, _second;

		public LinkedExpander(IExpander first, IExpander second)
		{
			_first  = first;
			_second = second;
		}

		public IRegistration Get(IIncludes parameter) => _first.Get(parameter).Then(_second.Get(parameter));
	}

	sealed class FixedExpander : IExpander
	{
		readonly IRegistration _context;

		public FixedExpander(IRegistration context) => _context = context;

		public IRegistration Get(IIncludes parameter) => _context;
	}

	sealed class TypeExpander<T> : TypeExpander
	{
		public TypeExpander(IServiceCollection services) : base(services, A.Type<T>()) {}
	}

	class TypeExpander : IExpander
	{
		readonly IServiceCollection _services;
		readonly Type               _subject;

		public TypeExpander(IServiceCollection services, Type subject)
		{
			_services = services;
			_subject  = subject;
		}

		public IRegistration Get(IIncludes parameter) => new TypesRegistration(_services, parameter.Get(_subject));
	}

	public sealed class Registration<T> : RegistrationWithInclude where T : class
	{
		public Registration(IServiceCollection subject, IRegistration current) : base(subject, current) {}

		public Registration(IServiceCollection subject, IExpander expander) : base(subject, expander) {}

		public Registration<T> Decorate<TNext>() where TNext : class, T
			=> new Registration<T>(Services, Next(new TypeExpander<TNext>(Services)
				                                      .Then(new Decorate<T, TNext>(Services))
			                                     ));
	}

	sealed class Register<T> : IRegistration where T : class
	{
		readonly IServiceCollection _subject;

		public Register(IServiceCollection subject) => _subject = subject;

		public RegistrationResult Singleton() => _subject.AddSingleton<T>().Result();

		public RegistrationResult Transient() => _subject.AddTransient<T>().Result();

		public RegistrationResult Scoped() => _subject.AddScoped<T>().Result();
	}

	sealed class Forward<T, TTo> : IRegistration where T : class where TTo : class, T
	{
		readonly IServiceCollection _subject;

		public Forward(IServiceCollection subject) => _subject = subject;

		public RegistrationResult Singleton() => _subject.AddSingleton<T, TTo>().Result();

		public RegistrationResult Transient() => _subject.AddTransient<T, TTo>().Result();

		public RegistrationResult Scoped() => _subject.AddScoped<T, TTo>().Result();
	}

	sealed class Decorate<T, TNext> : IRegistration
		where T : class where TNext : class, T
	{
		readonly IServiceCollection _subject;

		public Decorate(IServiceCollection subject) => _subject = subject;

		public RegistrationResult Singleton() => _subject.Decorate<T, TNext>().Result();

		public RegistrationResult Transient() => _subject.Decorate<T, TNext>().Result();

		public RegistrationResult Scoped() => _subject.Decorate<T, TNext>().Result();
	}
}