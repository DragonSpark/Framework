using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
using DragonSpark.Runtime.Environment;
using LightInject;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Composition
{
	public static class Extensions
	{
		public static BuildHostContext Host(this ModelContext _)
			=> Start.A.Selection.Of<IHostBuilder>().By.Self.Get().To(Start.An.Extent<BuildHostContext>());

		public static IAlteration<IServiceCollection> Option<T>(this VowelContext _) where T : class, new()
			=> RegisterOption<T>.Default;

		public static HostOperationsContext Operations(this BuildHostContext @this) => new HostOperationsContext(@this);

		public static IConfiguration Configuration(this IServiceCollection @this)
			=> @this.Single(x => x.ServiceType == typeof(IConfiguration))
			        .ImplementationFactory?.Invoke(null)
			        .To<IConfiguration>();

		public static T GetInstance<T>(this IServiceCollection @this) where T : class
			=> (@this.Where(x => x.ServiceType == typeof(T))
			         .Select(x => x.ImplementationInstance)
			         .Only()
			    ??
			    @this.Select(x => x.ImplementationInstance)
			         .OfType<T>()
			         .FirstOrDefault())?
				.To<T>();

		public static T GetRequiredInstance<T>(this IServiceCollection @this) where T : class
			=> (@this.Where(x => x.ServiceType == typeof(T))
			         .Select(x => x.ImplementationInstance)
			         .Only()
			    ??
			    @this.Select(x => x.ImplementationInstance)
			         .OfType<T>()
			         .FirstOrDefault()
			   )
				.To<T>();

		public static RegistrationContext<T> For<T>(this IServiceCollection @this) where T : class
			=> new RegistrationContext<T>(@this);

		public static RegistrationContext ForDefinition<T>(this IServiceCollection @this) where T : class
			=> new RegistrationContext(@this, A.Type<T>().GetGenericTypeDefinition());

		public static BuildHostContext WithComposition(this BuildHostContext @this)
			=> @this.Select(Composition.WithComposition.Default);

		public static BuildHostContext WithDefaultComposition(this BuildHostContext @this)
			=> @this.ComposeUsing<ConfigureDefaultActivation>();

		public static BuildHostContext RegisterModularity(this BuildHostContext @this)
			=> @this.Configure(Composition.RegisterModularity.Default);

		public static BuildHostContext RegisterModularity<T>(this BuildHostContext @this)
			where T : class, IActivateUsing<Assembly>, IArray<Type>
			=> @this.Configure(new RegisterModularity(TypeSelection<T>.Default.Get));

		public static BuildHostContext ConfigureFromEnvironment(this BuildHostContext @this)
			=> @this.WithComposition().Configure(Compose.ConfigureFromEnvironment.Default);

		public static ICommand<IServiceCollection> ConfigureFromEnvironment(
			this ICommand<IServiceCollection> @this) => @this.Then().Add(Compose.ConfigureFromEnvironment.Default).Get();

		public static BuildHostContext ComposeUsingRoot<T>(this BuildHostContext @this)
			where T : ICompositionRoot, new()
			=> @this.WithComposition().Configure(ConfigureContainer<T>.Default);

		public static BuildHostContext ComposeUsing<T>(this BuildHostContext @this)
			where T : class, ICommand<IServiceContainer>
			=> @this.ComposeUsing(Start.An.Activation<T>().Activate());

		public static BuildHostContext ComposeUsing(this BuildHostContext @this, ICommand<IServiceContainer> configure)
			=> @this.ComposeUsing(configure.Execute);

		public static BuildHostContext ComposeUsing(this BuildHostContext @this, Action<IServiceContainer> configure)
			=> @this.WithComposition().Configure(new ConfigureContainer(configure));

		/*
		public static IServiceRegistry DecorateWithDependencies<TFrom, TTo>(this IServiceRegistry @this)
			where TTo : TFrom
			=> @this.Decorate<TFrom, TTo>()
			        .RegisterDependencies(typeof(TTo));

		public static IServiceRegistry DecorateDefinition<TFrom, TTo>(this IServiceRegistry @this) where TTo : TFrom
		{
			var to = typeof(TTo).GetGenericTypeDefinition();
			return @this.Register(to)
			            .Decorate(typeof(TFrom).GetGenericTypeDefinition(), to)
			            .RegisterDependencies(to);
		}

		public static IServiceRegistry RegisterWithDependencies<T>(this IServiceRegistry @this)
			=> @this.Register<T>()
			        .RegisterDependencies(typeof(T));

		public static IServiceRegistry RegisterWithDependencies<TFrom, TTo>(this IServiceRegistry @this)
			where TTo : TFrom
			=> @this.Register<TFrom, TTo>()
			        .RegisterDependencies(typeof(TTo));

		public static IServiceRegistry RegisterDependencies(this IServiceRegistry @this, Type type)
			=> new RegisterDependencies(type).Get(@this);*/
	}

	/*public interface IRegistration : IAlteration<IServiceRegistry> {}*/

	/*class DecoratedRegistration : Alteration<IServiceRegistry>,
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
	}*/

	/*sealed class Registration<TFrom, TTo> : MappedRegistration where TTo : class, TFrom
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
	}*/
}