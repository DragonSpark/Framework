using DragonSpark.Application;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Collections;
using DragonSpark.Reflection.Members;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime.Activation;
using DragonSpark.Runtime.Environment;
using LightInject;
using System;
using System.Linq;

namespace DragonSpark.Composition
{
	class Services : ServiceContainer, IServices, IActivateUsing<ContainerOptions>
	{
		public Services() : this(ContainerOptions.Default) {}

		public Services(ContainerOptions options) : base(options) {}

		public object GetService(Type serviceType) => GetInstance(serviceType);
	}

	sealed class ServiceOptions : Component<ContainerOptions>
	{
		public static ServiceOptions Default { get; } = new ServiceOptions();

		ServiceOptions() : base(new ContainerOptions {EnablePropertyInjection = false}.Self) {}
	}

	sealed class ServiceConfiguration : Component<ICommand<IServices>>
	{
		public static ServiceConfiguration Default { get; } = new ServiceConfiguration();

		ServiceConfiguration() : base(EmptyCommand<IServices>.Default.Self) {}
	}

	public interface IRegistration : IAlteration<IServiceRegistry> {}

	class DecoratedRegistration : Alteration<IServiceRegistry>,
	                              IRegistration,
	                              IActivateUsing<IAlteration<IServiceRegistry>>
	{
		public DecoratedRegistration(ISelect<IServiceRegistry, IServiceRegistry> registration) : base(registration) {}
	}

	sealed class InstanceRegistration<T> : IRegistration, IActivateUsing<T>
	{
		readonly T      _instance;
		readonly string _name;

		public InstanceRegistration(T instance) : this(instance, instance.GetType().AssemblyQualifiedName) {}

		public InstanceRegistration(T instance, string name)
		{
			_instance = instance;
			_name     = name;
		}

		public IServiceRegistry Get(IServiceRegistry parameter) => parameter.RegisterInstance(_instance, _name);
	}

	sealed class InstanceRegistration : IRegistration, IActivateUsing<object>
	{
		readonly Type   _type;
		readonly object _instance;

		public InstanceRegistration(object instance) : this(instance.GetType(), instance) {}

		public InstanceRegistration(Type type, object instance)
		{
			_type     = type;
			_instance = instance;
		}

		public IServiceRegistry Get(IServiceRegistry parameter) => parameter.RegisterInstance(_type, _instance);
	}

	sealed class Registration<TFrom, TTo> : MappedRegistration where TTo : class, TFrom
	{
		public static Registration<TFrom, TTo> Default { get; } = new Registration<TFrom, TTo>();

		Registration() : base(typeof(TFrom), typeof(TTo)) {}
	}

	sealed class RegisterWithDependencies<TFrom, TTo> : CompositeRegistration where TTo : class, TFrom
	{
		public static RegisterWithDependencies<TFrom, TTo> Default { get; } =
			new RegisterWithDependencies<TFrom, TTo>();

		RegisterWithDependencies() : base(new MappedRegistration(typeof(TFrom), typeof(TTo)),
		                                  RegisterDependencies<TTo>.Default) {}
	}

	sealed class RegisterDependencies<T> : DecoratedRegistration
	{
		public static RegisterDependencies<T> Default { get; } = new RegisterDependencies<T>();

		RegisterDependencies() : base(new RegisterDependencies(typeof(T))) {}
	}

	sealed class RegisterDependencies : IRegistration
	{
		readonly static Func<IServiceRegistry, Func<Type, bool>> Where = CanRegister.Default.Get;

		readonly Array<Type>                              _candidates;
		readonly Func<IServiceRegistry, Func<Type, bool>> _where;

		public RegisterDependencies(Type type) : this(new DependencyCandidates(type).Get(type), Where) {}

		public RegisterDependencies(Array<Type> candidates, Func<IServiceRegistry, Func<Type, bool>> where)
		{
			_candidates = candidates;
			_where      = where;
		}

		public IServiceRegistry Get(IServiceRegistry parameter)
			=> _candidates.Open()
			              .Where(_where(parameter))
			              .Aggregate(parameter, (repository, t) => repository.Register(t)
			                                                                 .RegisterDependencies(t));
	}

	sealed class DependencyCandidates : ArrayStore<Type, Type>, IActivateUsing<Type>
	{
		public DependencyCandidates(Type type) : base(A.This(TypeMetadata.Default)
		                                               .Select(Constructors.Default)
		                                               .Query()
		                                               .SelectMany(Parameters.Default.Open())
		                                               .Select(ParameterType.Default)
		                                               .Select(new GenericTypeDependencySelector(type))
		                                               .Where(IsClass.Default)
		                                               .Out()) {}
	}

	class MappedRegistration : IRegistration
	{
		readonly Type _from;
		readonly Type _to;

		public MappedRegistration(Type from, Type to)
		{
			_from = from;
			_to   = to;
		}

		public IServiceRegistry Get(IServiceRegistry parameter) => parameter.Register(_from, _to);
	}

	sealed class CanRegister : Select<IServiceRegistry, Func<Type, bool>>
	{
		public static CanRegister Default { get; } = new CanRegister();

		CanRegister() : base(Start.A.Selection.Of<IServiceRegistry>()
		                          .By.Calling(x => x.AvailableServices)
		                          .Query()
		                          .Select(ServiceTypeSelector.Default)
		                          .Out()
		                          .Then()
		                          .Activate<NotHave<Type>>()
		                          .Get()
		                          .AsDefined()
		                          .Then()
		                          .Delegate()) {}
	}

	sealed class ServiceTypeSelector : Select<ServiceRegistration, Type>
	{
		public static ServiceTypeSelector Default { get; } = new ServiceTypeSelector();

		ServiceTypeSelector() : base(x => x.ServiceType) {}
	}

	class Registration : IRegistration
	{
		readonly Type _type;

		public Registration(Type type) => _type = type;

		public IServiceRegistry Get(IServiceRegistry parameter) => parameter.Register(_type);
	}

	sealed class Registration<T> : MappedRegistration
	{
		public Registration(Type type) : base(typeof(T), type) {}
	}

	class CompositeRegistration : IRegistration, IActivateUsing<Array<IRegistration>>
	{
		readonly Array<IRegistration> _configurations;

		public CompositeRegistration(params IRegistration[] configurations) : this(configurations.Result()) {}

		public CompositeRegistration(Array<IRegistration> configurations) => _configurations = configurations;

		public IServiceRegistry Get(IServiceRegistry parameter) => _configurations.Open().Alter(parameter);
	}
}