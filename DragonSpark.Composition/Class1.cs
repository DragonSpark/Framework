﻿using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Reflection.Selection;
using DragonSpark.Runtime.Activation;
using DragonSpark.Runtime.Environment;
using JetBrains.Annotations;
using LightInject;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Composition
{
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

	public interface IServiceConfiguration : ICommand<IServiceCollection> {}

	public class ServiceConfiguration : Command<IServiceCollection>, IServiceConfiguration
	{
		public ServiceConfiguration(ICommand<IServiceCollection> command) : base(command) {}

		public ServiceConfiguration(Action<IServiceCollection> command) : base(command) {}
	}

	public interface IContainerConfiguration : ICommand<IServiceContainer> {}

	public sealed class ContainerConfiguration : Command<IServiceContainer>, IContainerConfiguration
	{
		public ContainerConfiguration(Action<IServiceContainer> command) : base(command) {}
	}

	sealed class RegisterModularity : IServiceConfiguration
	{
		[UsedImplicitly]
		public static RegisterModularity Default { get; } = new RegisterModularity();

		RegisterModularity() : this(TypeSelection<PublicAssemblyTypes>.Default.Open().Get) {}

		readonly Func<IReadOnlyList<Type>, IComponentTypes>         _locator;
		readonly Func<IReadOnlyList<Assembly>, IReadOnlyList<Type>> _types;
		readonly Func<string, IReadOnlyList<Assembly>>              _select;

		public RegisterModularity(Func<IReadOnlyList<Assembly>, IReadOnlyList<Type>> types)
			: this(ComponentTypeLocators.Default.Get, types, EnvironmentAwareAssemblies.Default.Open().Get) {}

		public RegisterModularity(Func<IReadOnlyList<Type>, IComponentTypes> locator,
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

	sealed class name
	{
		
	}

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
			parameter.RegisterInstance(parameter)
			         .RegisterInstance(parameter.To<ServiceContainer>().ConstructorSelector)
			         .RegisterFallback(_condition, _select);
		}
	}
}