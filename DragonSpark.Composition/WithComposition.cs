using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using DragonSpark.Runtime.Activation;
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
using Array = DragonSpark.Model.Sequences.Array;

namespace DragonSpark.Composition
{
	sealed class WithComposition : ReferenceValueStore<IHostBuilder, IHostBuilder>, IAlteration<IHostBuilder>
	{
		public static WithComposition Default { get; } = new WithComposition();

		WithComposition() : base(Select.Instance) {}

		sealed class Select : IAlteration<IHostBuilder>
		{
			public static Select Instance { get; } = new Select();
			readonly FieldInfo _provider;

			Select() : this(typeof(ServiceContainer).GetField("constructionInfoProvider",
			                                                  BindingFlags.Instance | BindingFlags.NonPublic)) {}

			public Select(FieldInfo provider) => _provider = provider;

			public IHostBuilder Get(IHostBuilder parameter)
			{
				var root = new ServiceContainer(ContainerOptions.Default.Clone()
				                                                .WithMicrosoftSettings()
				                                                .WithAspNetCoreSettings());
				root.ConstructorDependencySelector = new Selector(root);

				var current = _provider.GetValue(root).To<Lazy<IConstructionInfoProvider>>();

				_provider.SetValue(root, new Lazy<IConstructionInfoProvider>(new Construction(current.Value)));

				var @default = new LightInjectServiceProviderFactory(root);
				var factory  = new Factory(@default);
				var result   = parameter.UseServiceProviderFactory(factory);
				return result;
			}

			sealed class Construction : IConstructionInfoProvider
			{
				readonly IConstructionInfoProvider _provider;
				readonly ICondition<Type> _condition;
				readonly IActivator _activator;

				public Construction(IConstructionInfoProvider provider)
					: this(provider, CanActivate.Default, Activator.Default) {}

				public Construction(IConstructionInfoProvider provider, ICondition<Type> condition,
				                    IActivator activator)
				{
					_provider = provider;
					_condition = condition;
					_activator = activator;
				}

				public ConstructionInfo GetConstructionInfo(Registration registration)
				{
					try
					{
						return _provider.GetConstructionInfo(registration);
					}
					catch (InvalidOperationException)
					{
						if (_condition.Get(registration.ImplementingType))
						{
							var instance = _activator.Get(registration.ImplementingType);
							registration.FactoryExpression = new Func<IServiceFactory, object>(instance.Accept);
							if (registration is ServiceRegistration service)
							{
								service.Value = instance;
							}
							return new ConstructionInfo{FactoryDelegate = registration.FactoryExpression};
						}

						throw;
					}
				}
			}
		}

		/*sealed class Constructors : IConstructorSelector
		{
			readonly IConstructorSelector _selector;
			readonly ICondition<Type> _condition;
			readonly IActivator _activator;

			public Constructors(IConstructorSelector selector)
				: this(selector, CanActivate.Default, Activator.Default) {}

			public Constructors(IConstructorSelector selector, ICondition<Type> condition, IActivator activator)
			{
				_selector = selector;
				_condition = condition;
				_activator = activator;
			}

			public ConstructorInfo Execute(Type implementingType)
			{
				try
				{
					return _selector.Execute(implementingType);
				}
				catch (InvalidOperationException)
				{
					if (_condition.Get(implementingType))
					{
						return new Construcinf;
					}

					throw;
				}
			}
		}*/

		sealed class Selector : IConstructorDependencySelector
		{
			readonly IServiceRegistry               _registry;
			readonly IConstructorDependencySelector _selector;
			readonly Array<Type>                    _types;

			public Selector(ServiceContainer container) : this(container.ConstructorDependencySelector, container,
			                                                   Array.Of(typeof(Func<,>), typeof(Func<,,>))) {}

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
			readonly IActivator       _activator;
			readonly ICondition<Type> _condition;
			readonly IServiceProvider _provider;

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
}