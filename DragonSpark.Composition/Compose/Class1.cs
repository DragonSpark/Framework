using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.DependencyInjection;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;

namespace DragonSpark.Composition.Compose
{
	static class FrameworkExtensions
	{
		public static IRegistration Then(this IRegistration @this, IRegistration next)
			=> new LinkedRegistrationContext(@this, next);

		public static IExpander Then(this IExpander @this, IRegistrationContext context) => @this.Then(context.Adapt());

		public static IExpander Then(this IExpander @this, IRegistration context) => @this.Then(context.Fixed());

		public static IExpander Then(this IExpander @this, IExpander next) => new LinkedExpander(@this, next);

		public static IExpander Fixed(this IRegistration @this) => new FixedExpander(@this);

		public static IRegistration Adapt(this IRegistrationContext @this) => new Adapter(@this);

		public static RegistrationResult Result(this IServiceCollection @this) => new RegistrationResult(@this);


	}

	public readonly struct RegistrationResult
	{
		public RegistrationResult(IServiceCollection then) => Then = then;

		public IServiceCollection Then { get; }
	}

	public interface IIncludeAwareRegistration : IRegistration
	{
		IRegistration Include(IRelatedTypes related);
	}

	public interface IRegistration
	{
		RegistrationResult Singleton();

		RegistrationResult Transient();

		RegistrationResult Scoped();
	}

	public sealed class StartRegistration<T> : IIncludeAwareRegistration where T : class
	{
		readonly IServiceCollection _subject;

		public StartRegistration(IServiceCollection subject) => _subject = subject;

		IExpander Current => new Register<T>(_subject).Adapt().Fixed();

		IExpander Expanded => new TypeExpander<T>(_subject).Then(Current);

		public Registrations And<TNext>() where TNext : class
			=> new Registrations(_subject,
			                     Expanded.Then(new TypeExpander<TNext>(_subject)
				                                   .Then(new Register<TNext>(_subject))));

		public Registration<T> Forward<TTo>() where TTo : class, T
			=> new Registration<T>(_subject,
			                       new TypeExpander<TTo>(_subject).Then(new Forward<T, TTo>(_subject)));

		public Registration<T> Use<TResult>() where TResult : class, IResult<T>
			=> new Registration<T>(_subject,
			                       new TypeExpander<TResult>(_subject)
				                       .Then(new ResultRegistration<T, TResult>(_subject)));

		public Registration<T> UseEnvironment()
			=> new Registration<T>(_subject, new SelectedRegistration<T>(_subject));

		public IRegistration Include(Func<RelatedTypesHolster, IRelatedTypes> related)
			=> Include(related(RelatedTypesHolster.Default));

		public IRegistration Include(IRelatedTypes related) => Expanded.Get(related.Get(_subject));

		public RegistrationResult Singleton() => Include(x => x.None).Singleton();

		public RegistrationResult Transient() => Include(x => x.None).Transient();

		public RegistrationResult Scoped() => Include(x => x.None).Scoped();
	}

	public sealed class GenericDefinitionRegistration<T> : GenericDefinitionRegistration
	{
		public GenericDefinitionRegistration(IServiceCollection services)
			: base(services, A.Type<T>().GetGenericTypeDefinition()) {}
	}

	public class GenericDefinitionRegistration : IncludeAwareRegistration
	{
		public GenericDefinitionRegistration(IServiceCollection services, Type definition)
			: base(services,
			       new TypeExpander(services, definition).Then(new TypeRegistration(services, definition))) {}
	}

	public class IncludeAwareRegistration : IIncludeAwareRegistration
	{
		readonly IExpander _current;

		public IncludeAwareRegistration(IServiceCollection services, IRegistrationContext context)
			: this(services, context.Adapt()) {}

		public IncludeAwareRegistration(IServiceCollection services, IRegistration next)
			: this(services, next.Fixed()) {}

		public IncludeAwareRegistration(IServiceCollection services, IExpander current)
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

		public RegistrationResult Singleton() => Include(x => x.None).Singleton();

		public RegistrationResult Transient() => Include(x => x.None).Transient();

		public RegistrationResult Scoped() => Include(x => x.None).Scoped();
	}

	public sealed class Registrations : IncludeAwareRegistration
	{
		public Registrations(IServiceCollection services, IExpander current) : base(services, current) {}

		public Registrations And<TNext>() where TNext : class
			=> new Registrations(Services, Next(new TypeExpander<TNext>(Services).Then(new Register<TNext>(Services))));
	}

	public sealed class RelatedTypesHolster
	{
		public static RelatedTypesHolster Default { get; } = new RelatedTypesHolster();

		RelatedTypesHolster() : this(RelatedTypes.Default, Dependencies.Default) {}

		public RelatedTypesHolster(IRelatedTypes none, Dependencies dependencies)
		{
			None         = none;
			Dependencies = dependencies;
		}

		public IRelatedTypes None { get; }

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
			=> new RecursiveDependencyIncludes(new DependencyIncludes(parameter));
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
		readonly IIncludes _includes;

		public RecursiveDependencyIncludes(IIncludes includes) => _includes = includes;

		public Array<Type> Get(Type parameter) => Yield(parameter).AsValueEnumerable().Distinct().ToArray();

		IEnumerable<Type> Yield(Type current)
		{
			foreach (var type in _includes.Get(current).Open())
			{
				yield return type;

				foreach (var other in Yield(type))
				{
					yield return other;
				}
			}
		}
	}

	sealed class DependencyIncludes : IIncludes
	{
		readonly Predicate<Type>    _can;
		readonly IArray<Type, Type> _candidates;

		public DependencyIncludes(IServiceCollection services)
			: this(new CanRegister(services).Then().And(new HashSet<Type>().Add), DependencyCandidates.Default) {}

		public DependencyIncludes(Predicate<Type> can, IArray<Type, Type> candidates)
		{
			_can        = can;
			_candidates = candidates;
		}

		public Array<Type> Get(Type parameter)
			=> _candidates.Get(parameter).Open().AsValueEnumerable().Where(_can).ToArray();
	}

	sealed class Includes : FixedResult<Type, Array<Type>>, IIncludes
	{
		public static Includes Default { get; } = new Includes();

		Includes() : base(Array<Type>.Empty) {}
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

	public sealed class Registration<T> : IncludeAwareRegistration where T : class
	{
		public Registration(IServiceCollection subject, IRegistrationContext context)
			: this(subject, context.Adapt()) {}

		public Registration(IServiceCollection subject, IRegistration current) : base(subject, current) {}

		public Registration(IServiceCollection subject, IExpander expander) : base(subject, expander) {}

		public Registration<T> Decorate<TNext>() where TNext : class, T
			=> new Registration<T>(Services, Next(new TypeExpander<TNext>(Services)
				                                      .Then(new Decorate<T, TNext>(Services))
			                                     ));
	}

	sealed class Register<T> : IRegistrationContext where T : class
	{
		readonly IServiceCollection _subject;

		public Register(IServiceCollection subject) => _subject = subject;

		public IServiceCollection Singleton() => _subject.AddSingleton<T>();

		public IServiceCollection Transient() => _subject.AddTransient<T>();

		public IServiceCollection Scoped() => _subject.AddScoped<T>();
	}

	sealed class Forward<T, TTo> : IRegistrationContext where T : class where TTo : class, T
	{
		readonly IServiceCollection _subject;

		public Forward(IServiceCollection subject) => _subject = subject;

		public IServiceCollection Singleton() => _subject.AddSingleton<T, TTo>();

		public IServiceCollection Transient() => _subject.AddTransient<T, TTo>();

		public IServiceCollection Scoped() => _subject.AddScoped<T, TTo>();
	}

	sealed class Decorate<T, TNext> : IRegistrationContext
		where T : class where TNext : class, T
	{
		readonly IServiceCollection _subject;

		public Decorate(IServiceCollection subject) => _subject = subject;

		public IServiceCollection Singleton() => _subject.Decorate<T, TNext>();

		public IServiceCollection Transient() => _subject.Decorate<T, TNext>();

		public IServiceCollection Scoped() => _subject.Decorate<T, TNext>();
	}

	sealed class Adapter : IRegistration
	{
		readonly IRegistrationContext _registration;

		public Adapter(IRegistrationContext registration) => _registration = registration;

		public RegistrationResult Singleton() => _registration.Singleton().Result();

		public RegistrationResult Transient() => _registration.Transient().Result();

		public RegistrationResult Scoped() => _registration.Scoped().Result();
	}
}