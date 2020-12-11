using AutoBogus;
using AutoBogus.Conventions;
using Bogus;
using DragonSpark.Application.Entities.Generation;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Reflection.Members;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Reflection;

namespace DragonSpark.Application.Compose.Entities.Generation
{
	public static class Extensions
	{
		public static GeneratorContext<T> Generator<T>(this ModelContext _)
			where T : class => new GeneratorContext<T>();
	}

	public interface IRule<T, out TOther> : ISelect<(Faker, T), TOther> where TOther : class {}

	sealed class Assign<T, TOther> : ICommand<(Faker, T, TOther)>
	{
		readonly Action<Faker, T, TOther> _previous;
		readonly Action<TOther, T>        _assign;

		public Assign(Action<Faker, T, TOther> previous, Action<TOther, T> assign)
		{
			_previous = previous;
			_assign   = assign;
		}

		public void Execute((Faker, T, TOther) parameter)
		{
			var (values, owner, instance) = parameter;
			_previous(values, owner, instance);
			_assign(instance, owner);
		}
	}

	sealed class Rule<T, TOther> : IRule<T, TOther> where TOther : class
	{
		readonly Func<Faker<TOther>, T, TOther> _generate;
		readonly Action<Faker, T, TOther>       _post;
		readonly Faker<TOther>                  _generator;

		public Rule(Faker<TOther> generator, Func<Faker<TOther>, T, TOther> generate, Action<Faker, T, TOther> post)
		{
			_generate  = generate;
			_post      = post;
			_generator = generator;
		}

		public TOther Get((Faker, T) parameter)
		{
			var (faker, owner) = parameter;
			var result = _generate(_generator, owner);
			_post(faker, owner, result);
			return result;
		}
	}

	/**/

	sealed class LocateAssignments<T, TOther> : ReferenceValueStore<string, IResult<Action<T, TOther>?>>
	{
		public static LocateAssignments<T, TOther> Default { get; } = new LocateAssignments<T, TOther>();

		LocateAssignments() : base(Start.A.Selection<string>()
		                                .By.Calling(string.Intern)
		                                .Select(x => new PrincipalPropertyByName(x))
		                                .Select(x => new LocatePrincipalProperty<T, TOther>(x))
		                                .Select(x => new LocateAssignment<T, TOther>(x))) {}
	}

	sealed class LocateAssignment<T, TValue> : IResult<Action<T, TValue>?>
	{
		public static LocateAssignment<T, TValue> Default { get; } = new LocateAssignment<T, TValue>();

		LocateAssignment() : this(LocatePrincipalProperty<T, TValue>.Default) {}

		readonly ILocatePrincipalProperty               _property;
		readonly IPropertyAssignmentDelegate<T, TValue> _delegates;

		public LocateAssignment(ILocatePrincipalProperty property)
			: this(property, PropertyAssignmentDelegates<T, TValue>.Default) {}

		public LocateAssignment(ILocatePrincipalProperty property, IPropertyAssignmentDelegate<T, TValue> delegates)
		{
			_property  = property;
			_delegates = delegates;
		}

		public Action<T, TValue>? Get()
		{
			var propertyInfo = _property.Get(A.Type<T>());
			var result       = propertyInfo != null ? _delegates.Get(propertyInfo) : null;
			return result;
		}
	}

	public interface ILocatePrincipalProperty : ISelect<Type, PropertyInfo?> {}

	sealed class LocatePrincipalProperty<T, TValue> : LocatePrincipalProperty
	{
		public static LocatePrincipalProperty<T, TValue> Default { get; } = new LocatePrincipalProperty<T, TValue>();

		LocatePrincipalProperty() : this(PrincipalProperty<T, TValue>.Default) {}

		public LocatePrincipalProperty(IPrincipalProperty property) : this(property, Start.A.Selection<PropertyInfo>()
		                                                                                  .By.Calling(x => x
			                                                                                              .PropertyType)
		                                                                                  .Select(Is.EqualTo(A.Type<
			                                                                                                     TValue
		                                                                                                     >()))
		                                                                                  .Out()
		                                                                                  .Then()) {}

		public LocatePrincipalProperty(IPrincipalProperty property, Predicate<PropertyInfo> filter) :
			base(property, filter) {}
	}

	class LocatePrincipalProperty : ILocatePrincipalProperty
	{
		readonly IPrincipalProperty       _property;
		readonly Predicate<PropertyInfo>  _filter;
		readonly MemoryPool<PropertyInfo> _pool;

		public LocatePrincipalProperty(IPrincipalProperty property, Predicate<PropertyInfo> filter)
			: this(property, filter, MemoryPool<PropertyInfo>.Shared) {}

		public LocatePrincipalProperty(IPrincipalProperty property, Predicate<PropertyInfo> filter,
		                               MemoryPool<PropertyInfo> pool)
		{
			_property = property;
			_filter   = filter;
			_pool     = pool;
		}

		public PropertyInfo? Get(Type parameter)
		{
			using var owner  = parameter.GetProperties().AsValueEnumerable().Where(_filter).ToArray(_pool);
			var       result = _property.Get(owner.Memory);
			return result;
		}
	}

	public interface IPrincipalProperty : ISelect<Memory<PropertyInfo>, PropertyInfo?> {}

	sealed class PrincipalProperty<T, TValue> : Select<Memory<PropertyInfo>, PropertyInfo?>, IPrincipalProperty
	{
		public static PrincipalProperty<T, TValue> Default { get; } = new PrincipalProperty<T, TValue>();

		PrincipalProperty() : base(Start.An.Instance(LocateOnlyPrincipalProperty.Default)
		                                .Then()
		                                .OrMaybe(PrincipalPropertyByName<TValue>.Default)
		                                .Get()
		                                .To(x => new MultipleCandidatePrincipalProperty<T, TValue>(x))) {}
	}

	sealed class MultipleCandidatePrincipalProperty<T, TValue> : ISelect<Memory<PropertyInfo>, PropertyInfo?>
	{
		readonly ISelect<Memory<PropertyInfo>, PropertyInfo?> _previous;

		public MultipleCandidatePrincipalProperty(ISelect<Memory<PropertyInfo>, PropertyInfo?> previous)
			=> _previous = previous;

		public PropertyInfo? Get(Memory<PropertyInfo> parameter)
		{
			var previous = _previous.Get(parameter);

			var result = previous == null && parameter.Length > 0
				             ? throw new
					               InvalidOperationException($"Could not locate a definitive property on type '{A.Type<T>()}' that is of type '{A.Type<TValue>()}'.  Manually select this property via the `Include` override.")
				             : previous;
			return result;
		}
	}

	sealed class LocateOnlyPrincipalProperty : ISelect<Memory<PropertyInfo>, PropertyInfo?>
	{
		public static LocateOnlyPrincipalProperty Default { get; } = new LocateOnlyPrincipalProperty();

		LocateOnlyPrincipalProperty() {}

		public PropertyInfo? Get(Memory<PropertyInfo> parameter) => parameter.Length == 1 ? parameter.Span[0] : default;
	}

	sealed class PrincipalPropertyByName<T> : PrincipalPropertyByName
	{
		public static PrincipalPropertyByName<T> Default { get; } = new PrincipalPropertyByName<T>();

		PrincipalPropertyByName() : base(A.Type<T>().Name) {}
	}

	class PrincipalPropertyByName : IPrincipalProperty
	{
		readonly string _name;

		public PrincipalPropertyByName(string name) => _name = name;

		public PropertyInfo? Get(Memory<PropertyInfo> parameter)
		{
			foreach (var info in parameter.Span)
			{
				if (info.Name == _name)
				{
					return info;
				}
			}

			return default;
		}
	}

	sealed class Generator<T> : IResult<AutoFaker<T>> where T : class
	{
		public static Generator<T> Default { get; } = new Generator<T>();

		Generator() : this(Configure<T>.Default.Execute) {}

		readonly Action<IAutoGenerateConfigBuilder> _configure;

		public Generator(Action<IAutoGenerateConfigBuilder> configure) => _configure = configure;

		public AutoFaker<T> Get() => new AutoFaker<T>().Configure(_configure);
	}

	sealed class Configure<T> : ICommand<IAutoGenerateConfigBuilder> where T : class
	{
		public static Configure<T> Default { get; } = new Configure<T>();

		Configure() : this(ModelBinder<T>.Default) {}

		readonly IAutoBinder _binder;

		public Configure(IAutoBinder binder) => _binder = binder;

		public void Execute(IAutoGenerateConfigBuilder parameter)
		{
			parameter.WithConventions().WithBinder(_binder);
		}
	}
}