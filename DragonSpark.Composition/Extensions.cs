using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Model.Commands;
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

		public static IServiceCollection Register<T>(this IServiceCollection @this) where T : class, new()
			=> RegisterOption<T>.Default.Get(@this);

		public static HostOperationsContext Operations(this BuildHostContext @this) => new HostOperationsContext(@this);

		public static IConfiguration Configuration(this IServiceCollection @this)
			=> @this.Single(x => x.ServiceType == typeof(IConfiguration))
			        .ImplementationFactory?.Invoke(null)
			        .To<IConfiguration>() ?? throw new InvalidOperationException();

		public static ComponentRequest Component<T>(this IServiceCollection @this)
		{
			var request = A.Type<T>();
			var result  = new ComponentRequest(request, @this.GetRequiredInstance<IComponentType>().Get(request));
			return result;
		}

		public static Func<T> Deferred<T>(this IServiceCollection @this) where T : class
			=> new DeferredService<T>(@this).Then().Singleton();

		public static T GetRequiredInstance<T>(this IServiceCollection @this) where T : class
			=> (@this.Where(x => x.ServiceType == typeof(T))
			         .Select(x => x.ImplementationInstance)
			         .Only()
			    ??
			    @this.Select(x => x.ImplementationInstance)
			         .OfType<T>()
			         .FirstOrDefault()
			   )!
				.To<T>();
/**/
		public static RegistrationContext<T> For<T>(this IServiceCollection @this) where T : class
			=> new RegistrationContext<T>(@this);

/**/
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
			this ICommand<IServiceCollection> @this)
			=> Compose.ConfigureFromEnvironment.Default.Then().Append(@this).Get();

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

		public static BuildHostContext Decorate<T>(this BuildHostContext @this, Func<IServiceFactory, T, T> configure)
			=> @this.ComposeUsing(new Decorate<T>(configure));

		public static BuildHostContext Decorate<TFrom, TTo>(this BuildHostContext @this) where TTo : TFrom
			=> @this.ComposeUsing(Composition.Decorate<TFrom, TTo>.Default);
	}
}