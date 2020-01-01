﻿using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using LightInject;
using LightInject.Microsoft.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace DragonSpark.Composition
{
	public static class Extensions
	{
		public static BuildHostContext Host(this ModelContext _)
			=> Start.A.Selection.Of<IHostBuilder>().By.Self.To(Start.An.Extent<BuildHostContext>());

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
			         .FirstOrDefault())
				.To<T>();

		/*sealed class Registration : IServiceConfiguration
		{
			readonly IServiceProviderFactory<IServiceContainer> _factory;

			public Registration(IServiceProviderFactory<IServiceContainer> factory) => _factory = factory;

			public void Execute(IServiceCollection parameter)
			{
				parameter.AddSingleton(_factory);
			}
		}*/

		static ContainerOptions WithoutVariance(this ContainerOptions @this)
		{
			@this.EnableVariance = false;
			return @this;
		}

		public static IHostBuilder UseLightInject(this IHostBuilder @this)
			=> @this.UseLightInject(ContainerOptions.Default.Clone().WithMicrosoftSettings().WithoutVariance());

		public static IHostBuilder UseLightInject(this IHostBuilder @this, ContainerOptions options)
			=> @this.UseLightInject(new LightInjectServiceProviderFactory(options));

		public static IHostBuilder UseLightInject(this IHostBuilder @this, IServiceContainer serviceContainer)
			=> @this.UseLightInject(new LightInjectServiceProviderFactory(serviceContainer));

		public static IHostBuilder UseLightInject(this IHostBuilder @this,
		                                          IServiceProviderFactory<IServiceContainer> factory)
			=> @this.UseServiceProviderFactory(factory);

		public static RegistrationContext<T> For<T>(this IServiceCollection @this) where T : class
			=> new RegistrationContext<T>(@this);

		public static IServiceRegistry RegisterDefinition<T>(this IServiceRegistry @this)
		{
			var to = typeof(T).GetGenericTypeDefinition();
			return @this.Register(to)
			            .RegisterDependencies(to);
		}

		/*public static IServiceRepository RegisterSingleton<T>(this IServiceRepository @this)
		{
			var to = typeof(T).GetGenericTypeDefinition();
			return @this.Register(to)
			            .RegisterDependencies(to);
		}*/

		public static IServiceRegistry RegisterDefinition<TFrom, TTo>(this IServiceRegistry @this) where TTo : TFrom
		{
			var to = typeof(TTo).GetGenericTypeDefinition();
			return @this.Register(to)
			            .Register(typeof(TFrom).GetGenericTypeDefinition(), to)
			            .RegisterDependencies(to);
		}

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
			=> new RegisterDependencies(type).Get(@this);
	}
}