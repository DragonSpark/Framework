using DragonSpark.Compose;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using DragonSpark.Reflection;
using DragonSpark.Runtime.Activation;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetFabric.Hyperlinq;
using System;
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

			Select() : this(typeof(ServiceContainer).GetField("constructionInfoProvider", PrivateInstanceFlags.Default)
			                                        .Verify()) {}

			readonly FieldInfo _provider;

			public Select(FieldInfo provider) => _provider = provider;

			public IHostBuilder Get(IHostBuilder parameter)
			{
				var options = ContainerOptions.Default.Clone().WithMicrosoftSettings().WithAspNetCoreSettings();
				var root    = new ServiceContainer(options);
				root.ConstructorSelector = new ConstructorSelector(new CanSelectDependency(root, options).Get);

				var current = _provider.GetValue(root).Verify().To<Lazy<IConstructionInfoProvider>>();

				_provider.SetValue(root, new Lazy<IConstructionInfoProvider>(new Construction(current.Value)));

				var @default = new LightInjectServiceProviderFactory(root);
				var factory  = new Factory(@default);
				var result   = parameter.UseServiceProviderFactory(factory);
				return result;
			}

			sealed class CanSelectDependency : AllCondition<ParameterInfo>
			{
				public CanSelectDependency(ServiceContainer owner, ContainerOptions options)
					: this(new CanLocateDependency(owner.CanGetInstance, options.EnableOptionalArguments),
					       new IsValidDependency(owner, Array.Of(typeof(Func<,>), typeof(Func<,,>)))) {}

				public CanSelectDependency(CanLocateDependency locate, IsValidDependency valid)
					: base(locate,
					       Start.A.Selection<ParameterInfo>().By.Calling(x => x.ParameterType).Select(valid).Out()) {}
			}

			sealed class CanLocateDependency : ICondition<ParameterInfo>
			{
				readonly Func<Type, string, bool> _specification;
				readonly bool                     _enableOptionalArguments;

				public CanLocateDependency(Func<Type, string, bool> specification, bool enableOptionalArguments = false)
				{
					_specification           = specification;
					_enableOptionalArguments = enableOptionalArguments;
				}

				public bool Get(ParameterInfo parameter) => _specification(parameter.ParameterType, string.Empty)
				                                            ||
				                                            !string.IsNullOrEmpty(parameter.Name)
				                                            &&
				                                            _specification(parameter.ParameterType, parameter.Name)
				                                            ||
				                                            _enableOptionalArguments && parameter.HasDefaultValue;
			}

			sealed class IsValidDependency : ICondition<Type>
			{
				readonly IServiceRegistry _registry;
				readonly Array<Type>      _types;

				public IsValidDependency(IServiceRegistry registry, Array<Type> types)
				{
					_registry = registry;
					_types    = types;
				}

				public bool Get(Type parameter)
					=> !parameter.IsConstructedGenericType || !_types.Open()
					                                                 .Contains(parameter.GetGenericTypeDefinition())
					                                       || _registry.AvailableServices.Introduce(parameter)
					                                                   .Any(x => x.Item1.ServiceType == x.Item2);
			}

			sealed class ConstructorSelector : IConstructorSelector
			{
				readonly Func<ParameterInfo, bool> _specification;

				public ConstructorSelector(Func<ParameterInfo, bool> specification) => _specification = specification;

				public ConstructorInfo Execute(Type implementingType)
				{
					var candidates = implementingType.GetTypeInfo()
					                                 .DeclaredConstructors.AsValueEnumerable()
					                                 .Where(c => c.IsPublic && !c.IsStatic)
					                                 .Where(x => x.Attribute<CandidateAttribute>()?.Enabled ?? true)
					                                 .ToArray();
					if (candidates.Length == 0)
					{
						throw new InvalidOperationException("Missing public constructor for Type: " +
						                                    implementingType.FullName);
					}

					foreach (var candidate in candidates.OrderByDescending(c => c.GetParameters().Length)
					                                    .AsValueEnumerable())
					{
						if (candidate!.GetParameters().All(_specification))
						{
							return candidate;
						}
					}

					throw new
						InvalidOperationException($"No resolvable constructor found for Type: {implementingType.FullName}");
				}
			}

			sealed class Construction : IConstructionInfoProvider
			{
				readonly IConstructionInfoProvider _provider;
				readonly ICondition<Type>          _condition;
				readonly IActivator                _activator;

				public Construction(IConstructionInfoProvider provider)
					: this(provider, CanActivate.Default, Activator.Default) {}

				public Construction(IConstructionInfoProvider provider, ICondition<Type> condition,
				                    IActivator activator)
				{
					_provider  = provider;
					_condition = condition;
					_activator = activator;
				}

				public ConstructionInfo? GetConstructionInfo(Registration registration)
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

							return new ConstructionInfo { FactoryDelegate = registration.FactoryExpression };
						}

						throw;
					}
				}
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

		sealed class Provider : IServiceProvider, IDisposable
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

			public object? GetService(Type serviceType)
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

			public void Dispose()
			{
				_provider.ToDisposable().Dispose();
			}
		}
	}
}