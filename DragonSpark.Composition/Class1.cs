using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection.Selection;
using DragonSpark.Runtime.Activation;
using DragonSpark.Runtime.Environment;
using JetBrains.Annotations;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetFabric.Hyperlinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Activator = DragonSpark.Runtime.Activation.Activator;

namespace DragonSpark.Composition
{
	sealed class WithComposition : ReferenceValueStore<IHostBuilder, IHostBuilder>, IAlteration<IHostBuilder>
	{
		public static WithComposition Default { get; } = new WithComposition();

		WithComposition() : base(Select.Instance) {}

		sealed class Select : IAlteration<IHostBuilder>
		{
			public static Select Instance { get; } = new Select();

			Select() {}

			public IHostBuilder Get(IHostBuilder parameter)
			{
				var root = new ServiceContainer(ContainerOptions.Default.Clone()
				                                                .WithMicrosoftSettings()
				                                                .WithAspNetCoreSettings());

				root.ConstructorDependencySelector = new Selector(root);

				var @default = new LightInjectServiceProviderFactory(root);
				var factory  = new Factory(@default);
				var result   = parameter.UseServiceProviderFactory(factory);
				return result;
			}
		}

		sealed class Selector : IConstructorDependencySelector
		{
			readonly IConstructorDependencySelector _selector;
			readonly IServiceRegistry               _registry;
			readonly Array<Type>                    _types;

			public Selector(ServiceContainer container)
				: this(container.ConstructorDependencySelector, container,
				       An.Array(typeof(Func<,>), typeof(Func<,,>))) {}

			public Selector(IConstructorDependencySelector selector, IServiceRegistry registry, Array<Type> types)
			{
				_selector = selector;
				_registry = registry;
				_types    = types;
			}

			public IEnumerable<ConstructorDependency> Execute(ConstructorInfo constructor)
			{
				var result = _selector.Execute(constructor).Open();

				foreach (var dependency in result)
				{
					var type = dependency.ServiceType;

					if (type.IsConstructedGenericType && _types.Open().Contains(type.GetGenericTypeDefinition()) &&
					    _registry.AvailableServices.Introduce(type).All(x => x.Item1.ServiceType != x.Item2))
					{
						throw new InvalidOperationException("Not supported.");
					}
				}

				return result;
			}
		}

		sealed class Factory : IServiceProviderFactory<IServiceContainer>
		{
			readonly IServiceProviderFactory<IServiceContainer> _factory;

			public Factory(IServiceProviderFactory<IServiceContainer> factory) => _factory = factory;

			public IServiceContainer CreateBuilder(IServiceCollection services) => _factory.CreateBuilder(services);

			public IServiceProvider CreateServiceProvider(IServiceContainer containerBuilder)
			{
				var result = new Provider(_factory.CreateServiceProvider(containerBuilder));
				containerBuilder.Decorate<IServiceProvider>((_, provider) => new Provider(provider));
				containerBuilder.Decorate<IServiceScopeFactory>((_, factory) => new ServiceScopeFactory(factory));
				return result;
			}
		}

		sealed class ServiceScopeFactory : IServiceScopeFactory
		{
			readonly IServiceScopeFactory _factory;

			public ServiceScopeFactory(IServiceScopeFactory factory) => _factory = factory;

			public IServiceScope CreateScope() => new Scope(_factory.CreateScope());

			sealed class Scope : IServiceScope
			{
				readonly IServiceScope _scope;

				public Scope(IServiceScope scope) => _scope = scope;

				public void Dispose()
				{
					_scope.Dispose();
				}

				public IServiceProvider ServiceProvider => new Provider(_scope.ServiceProvider);
			}
		}

		sealed class Provider : IServiceProvider
		{
			readonly IServiceProvider _provider;
			readonly ICondition<Type> _condition;
			readonly IActivator       _activator;

			public Provider(IServiceProvider provider) : this(provider, CanActivate.Default, Activator.Default) {}

			public Provider(IServiceProvider provider, ICondition<Type> condition, IActivator activator)
			{
				_provider  = provider;
				_condition = condition;
				_activator = activator;
			}

			public object GetService(Type serviceType)
			{
				try
				{
					return _provider.GetService(serviceType);
				}
				catch (InvalidOperationException)
				{
					if (_condition.Get(serviceType))
					{
						return _activator.Get(serviceType);
					}

					throw;
				}
			}
		}
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

	public interface IServiceConfiguration : ICommand<IServiceCollection> {}

	public class ServiceConfiguration : Command<IServiceCollection>, IServiceConfiguration
	{
		public ServiceConfiguration(ICommand<IServiceCollection> command) : base(command) {}

		public ServiceConfiguration(Action<IServiceCollection> command) : base(command) {}
	}

	public interface IContainerConfiguration : ICommand<IServiceContainer> {}

	public class ContainerConfiguration : Command<IServiceContainer>, IContainerConfiguration
	{
		public ContainerConfiguration(Action<IServiceContainer> command) : base(command) {}
	}

	sealed class RegisterModularity : IServiceConfiguration
	{
		[UsedImplicitly]
		public static RegisterModularity Default { get; } = new RegisterModularity();

		RegisterModularity() : this(TypeSelection<PublicAssemblyTypes>.Default.Get) {}

		readonly Func<Array<Type>, IComponentTypes> _locator;
		readonly Func<Array<Assembly>, Array<Type>> _types;
		readonly Func<string, Array<Assembly>>      _select;

		public RegisterModularity(Func<Array<Assembly>, Array<Type>> types)
			: this(ComponentTypeLocators.Default.Get, types, EnvironmentAwareAssemblies.Default.Get) {}

		public RegisterModularity(Func<Array<Type>, IComponentTypes> locator, Func<Array<Assembly>, Array<Type>> types,
		                          Func<string, Array<Assembly>> select)
		{
			_locator = locator;
			_types   = types;
			_select  = select;
		}

		public void Execute(IServiceCollection parameter)
		{
			var name       = parameter.GetRequiredInstance<IHostEnvironment>().EnvironmentName;
			var assemblies = _select(name);
			var types      = _types(assemblies);
			var locator    = _locator(types);

			parameter.AddSingleton<IArray<Assembly>>(new ArrayInstance<Assembly>(assemblies))
			         .AddSingleton<IArray<Type>>(new ArrayInstance<Type>(types))
			         .AddSingleton(locator)
			         .AddSingleton<IComponentType>(new ComponentType(locator));
		}
	}

	sealed class ConfigureDefaultActivation : ICommand<IServiceContainer>
	{
		[UsedImplicitly]
		public static ConfigureDefaultActivation Default { get; } = new ConfigureDefaultActivation();

		ConfigureDefaultActivation() : this(CanActivate.Default.Get,
		                                    Start.A.Selection<ServiceRequest>()
		                                         .By.Calling(x => x.ServiceType)
		                                         .Get()
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
			parameter.RegisterInstance(parameter)
			         .RegisterFallback(_condition, _select);
		}
	}
}