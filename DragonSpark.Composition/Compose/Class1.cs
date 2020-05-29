using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DragonSpark.Composition.Compose
{
	public static class Extensions
	{
		public static IRegistrationContext Then(this IRegistrationContext @this, IRegistrationContext next)
			=> new LinkedRegistrationContext(@this, next);

		public static IExpander Then(this IExpander @this, IRegistrationContext context) => @this.Then(context.Fixed());

		public static IExpander Then(this IExpander @this, IExpander next) => new LinkedExpander(@this, next);

		public static IExpander Fixed(this IRegistrationContext @this) => new FixedExpander(@this);

		public static RegistrationResult Result(this IServiceCollection @this) => new RegistrationResult(@this);
	}

	public readonly struct RegistrationResult
	{
		public RegistrationResult(IServiceCollection then) => Then = then;

		public IServiceCollection Then { get; }
	}

	public interface IRegistrationWithInclude
	{
		IRegistrationContext Include(IRelatedTypes related);

		RegistrationResult Singleton();

		RegistrationResult Transient();

		RegistrationResult Scoped();
	}

	public sealed class NewRegistrationContext<T> : RegistrationWithInclude where T : class
	{
		public NewRegistrationContext(IServiceCollection subject)
			: base(subject, new RegistrationContext(subject, A.Type<T>())) {}

		public CompositeRegistrationContext And<TNext>()
			=> new CompositeRegistrationContext(Services, Next(new TypeExpander<TNext>(Services)));

		public Registration<T> Forward<TTo>() where TTo : class, T
			=> new Registration<T>(Services, new Forward<T, TTo>(Services).Fixed());
	}

	public class RegistrationWithInclude : IRegistrationWithInclude
	{
		readonly IExpander _current;

		public RegistrationWithInclude(IServiceCollection services, IRegistrationContext next)
			: this(services, next.Fixed()) {}

		public RegistrationWithInclude(IServiceCollection services, IExpander current)
		{
			_current = current;
			Services = services;
		}

		protected IServiceCollection Services { get; }

		protected IExpander Next(IRegistrationContext next) => Next(next.Fixed());

		protected IExpander Next(IExpander next) => _current.Then(next);

		public IRegistrationContext Include(Func<RelatedTypesHolster, IRelatedTypes> related)
			=> Include(related(RelatedTypesHolster.Default));

		public IRegistrationContext Include(IRelatedTypes related) => _current.Get(related);

		public RegistrationResult Singleton() => Include(x => x.AsIs).Singleton().Result();

		public RegistrationResult Transient() => Include(x => x.AsIs).Transient().Result();

		public RegistrationResult Scoped() => Include(x => x.AsIs).Scoped().Result();
	}

	public sealed class CompositeRegistrationContext : RegistrationWithInclude
	{
		public CompositeRegistrationContext(IServiceCollection services, IExpander current) : base(services, current) {}

		public CompositeRegistrationContext And<TNext>()
			=> new CompositeRegistrationContext(Services, Next(new TypeExpander<TNext>(Services)));
	}

	public sealed class RelatedTypesHolster
	{
		public static RelatedTypesHolster Default { get; } = new RelatedTypesHolster();

		RelatedTypesHolster() : this(RelatedTypes.Default, null) {}

		public RelatedTypesHolster(IRelatedTypes asIs, IRelatedTypes dependencies)
		{
			AsIs         = asIs;
			Dependencies = dependencies;
		}

		public IRelatedTypes AsIs { get; }

		public IRelatedTypes Dependencies { get; }
	}

	public interface IRelatedTypes : IArray<Type, Type> {}

	class Dependencies : IRelatedTypes
	{
		public Array<Type> Get(Type parameter) => default; // TODO.
	}

	sealed class RelatedTypes : Select<Type, Array<Type>>, IRelatedTypes
	{
		public static RelatedTypes Default { get; } = new RelatedTypes();

		RelatedTypes() : base(x => x.Yield().Result()) {}
	}

	public sealed class LinkedRegistrationContext : IRegistrationContext
	{
		readonly IRegistrationContext _next;
		readonly IRegistrationContext _previous;

		public LinkedRegistrationContext(IRegistrationContext previous, IRegistrationContext next)
		{
			_previous = previous;
			_next     = next;
		}

		public IServiceCollection Singleton()
		{
			_previous.Singleton();
			var result = _next.Singleton();
			return result;
		}

		public IServiceCollection Transient()
		{
			_previous.Transient();
			var result = _next.Transient();
			return result;
		}

		public IServiceCollection Scoped()
		{
			_previous.Scoped();
			var result = _next.Scoped();
			return result;
		}
	}

	public sealed class TypesRegistration : IRegistrationContext
	{
		readonly IServiceCollection _services;
		readonly Array<Type>        _types;

		public TypesRegistration(IServiceCollection services, Array<Type> types)
		{
			_services = services;
			_types    = types;
		}

		public IServiceCollection Singleton()
		{
			var result = _services;
			var length = _types.Length;
			for (var i = 0; i < length; i++)
			{
				result = result.AddSingleton(_types[i]);
			}

			return result;
		}

		public IServiceCollection Transient()
		{
			var result = _services;
			var length = _types.Length;
			for (var i = 0; i < length; i++)
			{
				result = result.AddTransient(_types[i]);
			}

			return result;
		}

		public IServiceCollection Scoped()
		{
			var result = _services;
			var length = _types.Length;
			for (var i = 0; i < length; i++)
			{
				result = result.AddScoped(_types[i]);
			}

			return result;
		}
	}

	public interface IExpander : ISelect<IRelatedTypes, IRegistrationContext> {}

	sealed class LinkedExpander : IExpander
	{
		readonly IExpander _first;
		readonly IExpander _second;

		public LinkedExpander(IExpander first, IExpander second)
		{
			_first  = first;
			_second = second;
		}

		public IRegistrationContext Get(IRelatedTypes parameter) => _first.Get(parameter).Then(_second.Get(parameter));
	}

	sealed class FixedExpander : IExpander
	{
		readonly IRegistrationContext _context;

		public FixedExpander(IRegistrationContext context) => _context = context;

		public IRegistrationContext Get(IRelatedTypes parameter) => _context;
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

		public IRegistrationContext Get(IRelatedTypes parameter)
			=> new TypesRegistration(_services, parameter.Get(_subject));
	}

	public sealed class Registration<T> : RegistrationWithInclude where T : class
	{
		public Registration(IServiceCollection subject, IRegistrationContext current)
			: this(subject, current.Fixed()) {}

		public Registration(IServiceCollection subject, IExpander expander) : base(subject, expander) {}

		public Registration<T> Decorate<TNext>() where TNext : class, T
			=> new Registration<T>(Services, Next(new Decorate<T, TNext>(Services)));
	}

	public sealed class Forward<T, TTo> : IRegistrationContext where T : class where TTo : class, T
	{
		readonly IServiceCollection _subject;

		public Forward(IServiceCollection subject) => _subject = subject;

		public IServiceCollection Singleton() => _subject.AddSingleton<T, TTo>();

		public IServiceCollection Transient() => _subject.AddTransient<T, TTo>();

		public IServiceCollection Scoped() => _subject.AddScoped<T, TTo>();
	}

	public sealed class Decorate<T, TNext> : IRegistrationContext
		where T : class where TNext : class, T
	{
		readonly IServiceCollection _subject;

		public Decorate(IServiceCollection subject) => _subject = subject;

		public IServiceCollection Singleton() => _subject.Decorate<T, TNext>();

		public IServiceCollection Transient() => _subject.Decorate<T, TNext>();

		public IServiceCollection Scoped() => _subject.Decorate<T, TNext>();
	}
}