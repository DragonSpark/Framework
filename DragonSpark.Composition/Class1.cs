using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Collections;
using DragonSpark.Reflection.Members;
using DragonSpark.Reflection.Selection;
using DragonSpark.Reflection.Types;
using DragonSpark.Runtime.Activation;
using DragonSpark.Runtime.Environment;
using JetBrains.Annotations;
using LightInject;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Composition
{
	public interface IConfigurator
	{
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		void ConfigureServices(IServiceCollection services);
	}

	public interface IContainerConfiguration : ICommand<IServiceContainer> {}

	sealed class ContainerConfiguration : Command<IServiceContainer>, IContainerConfiguration
	{
		public ContainerConfiguration(Action<IServiceContainer> command) : base(command) {}
	}

	public static class ConfigureComposition
	{
		public static BuildHostContext WithComposition(this BuildHostContext @this)
			=> @this.Select(Composition.WithComposition.Default);

		public static BuildHostContext ComposeUsingRoot<T>(this BuildHostContext @this)
			where T : ICompositionRoot, new()
			=> @this.WithComposition().Configure(ConfigureContainer<T>.Default);

		public static BuildHostContext WithDefaultComposition(this BuildHostContext @this)
			=> @this.ComposeUsing<ConfigureDefaultActivation>();

		public static BuildHostContext RegisterModularity(this BuildHostContext @this)
			=> @this.Configure(ApplyModularity.Default);

		public static BuildHostContext RegisterModularity<T>(this BuildHostContext @this)
			where T : class, IActivateUsing<Assembly>, IArray<Type>
			=> @this.Configure(new ApplyModularity(TypeSelection<T>.Default.Open().Get));

		public static BuildHostContext ComposeUsing<T>(this BuildHostContext @this)
			where T : class, ICommand<IServiceContainer>
			=> @this.ComposeUsing(Start.An.Activation<T>().Activate());

		public static BuildHostContext ComposeUsing(this BuildHostContext @this, ICommand<IServiceContainer> configure)
			=> @this.ComposeUsing(configure.Execute);

		public static BuildHostContext ComposeUsing(this BuildHostContext @this, Action<IServiceContainer> configure)
			=> @this.WithComposition().Configure(new ConfigureContainer(configure));
	}

	sealed class WithComposition : ReferenceValueStore<IHostBuilder, IHostBuilder>, IAlteration<IHostBuilder>
	{
		public static WithComposition Default { get; } = new WithComposition();

		WithComposition() : base(x => x.UseLightInject()) {}
	}

	sealed class ConfigureContainer<T> : ConfigureContainer where T : ICompositionRoot, new()
	{
		public static ConfigureContainer<T> Default { get; } = new ConfigureContainer<T>();

		ConfigureContainer() : base(x => x.RegisterFrom<T>()) {}
	}

	class ConfigureContainer : ICommand<IHostBuilder>
	{
		readonly Action<IServiceContainer> _configure;

		public ConfigureContainer(Action<IServiceContainer> configure) => _configure = configure;

		public void Execute(IHostBuilder parameter)
		{
			parameter.ConfigureContainer(_configure);
		}
	}

	public class Configurator : IConfigurator
	{
		readonly Action<IServiceCollection> _configure;

		public Configurator(Action<IServiceCollection> configure) => _configure = configure;

		public void ConfigureServices(IServiceCollection services)
		{
			_configure(services);
		}
	}

	public interface IServiceConfiguration : ICommand<IServiceCollection> {}

	sealed class EmptyServiceConfiguration : IServiceConfiguration
	{
		public static EmptyServiceConfiguration Default { get; } = new EmptyServiceConfiguration();

		EmptyServiceConfiguration() {}

		public void Execute(IServiceCollection parameter) {}
	}

	public class ServiceConfiguration : Command<IServiceCollection>, IServiceConfiguration
	{
		public ServiceConfiguration(ICommand<IServiceCollection> command) : base(command) {}

		public ServiceConfiguration(Action<IServiceCollection> command) : base(command) {}
	}

	public class LocatedServiceConfiguration : Command<IServiceCollection>, IServiceConfiguration
	{
		public LocatedServiceConfiguration(ICommand<IServiceCollection> @default)
			: base( /*Start.A.Result.Of.Type<IServiceConfiguration>()
			            .By.Location.Or.Default(EmptyServiceConfiguration.Default)
			            .Assume()
			            .Then(@default)*/@default) {}
	}

	sealed class ApplyModularity : IServiceConfiguration
	{
		[UsedImplicitly]
		public static ApplyModularity Default { get; } = new ApplyModularity();

		ApplyModularity() : this(TypeSelection<PublicAssemblyTypes>.Default.Open().Get) {}

		readonly Func<IReadOnlyList<Type>, IComponentTypes>         _locator;
		readonly Func<IReadOnlyList<Assembly>, IReadOnlyList<Type>> _types;
		readonly Func<string, IReadOnlyList<Assembly>>              _select;

		public ApplyModularity(Func<IReadOnlyList<Assembly>, IReadOnlyList<Type>> types)
			: this(ComponentTypeLocators.Default.Get, types, EnvironmentAwareAssemblies.Default.Open().Get) {}

		public ApplyModularity(Func<IReadOnlyList<Type>, IComponentTypes> locator,
		                       Func<IReadOnlyList<Assembly>, IReadOnlyList<Type>> types,
		                       Func<string, IReadOnlyList<Assembly>> select)
		{
			_locator = locator;
			_types   = types;
			_select  = select;
		}

		public void Execute(IServiceCollection parameter)
		{
			var assemblies = _select(parameter.GetRequiredInstance<IHostEnvironment>().EnvironmentName);
			var types      = _types(assemblies);
			var locator    = _locator(types);

			parameter.AddSingleton(assemblies)
			         .AddSingleton(types)
			         .AddSingleton(locator)
			         .AddSingleton<IComponentType>(new ComponentType(locator));
		}
	}

	public interface IRegistration : IAlteration<IServiceRegistry> {}

	sealed class ConfigureDefaultActivation : ICommand<IServiceContainer>
	{
		[UsedImplicitly]
		public static ConfigureDefaultActivation Default { get; } = new ConfigureDefaultActivation();

		ConfigureDefaultActivation() : this(CanActivate.Default.Get,
		                                    Start.A.Selection<ServiceRequest>()
		                                         .By.Calling(x => x.ServiceType)
		                                         .Then()
		                                         .Activate()) {}

		readonly Func<Type, string, bool>     _condition;
		readonly Func<ServiceRequest, object> _select;

		public ConfigureDefaultActivation(Func<Type, string, bool> condition, Func<ServiceRequest, object> select)
		{
			_condition = condition;
			_select    = select;
		}

		public void Execute(IServiceContainer parameter)
		{
			parameter.RegisterFallback(_condition, _select);
		}
	}

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

	sealed class ServiceSelector<T> : Select<IServiceProvider, T>
	{
		public static ServiceSelector<T> Default { get; } = new ServiceSelector<T>();

		ServiceSelector() : base(x => x.Get<T>()) {}
	}

	sealed class GenericTypeDependencySelector : ValidatedAlteration<Type>, IActivateUsing<Type>
	{
		public GenericTypeDependencySelector(Type type)
			: base(Start.A.Selection.Of.System.Type.By.Returning(IsGenericTypeDefinition.Default.In(type)),
			       GenericTypeDefinition.Default.If(IsDefinedGenericType.Default)) {}
	}

	sealed class DependencyCandidates : ArrayStore<Type, Type>, IActivateUsing<Type>
	{
		public DependencyCandidates(Type type) : base(Start.An.Instance(TypeMetadata.Default)
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
		                          .Select<NotHave<Type>>()
		                          .Get()
		                          .DefinedAsCondition()
		                          .Then()
		                          .Delegate()
		                    ) {}
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