using DragonSpark.Compose;
using DragonSpark.Composition.Compose;
using DragonSpark.Composition.Compose.Deferred;
using DragonSpark.Model.Commands;
using DragonSpark.Runtime.Environment;
using LightInject;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace DragonSpark.Composition;

public static class Extensions
{
	public static BuildHostContext Host(this ModelContext _) => new();

	public static IServiceCollection Register<T>(this IServiceCollection @this) where T : class
		=> RegisterOption<T>.Default.Get(@this);

	extension(IConfiguration @this)
	{
		public T? Section<T>(string name) where T : class
			=> new Section<T>(name).Get(@this);

		public T? Section<T>() where T : class => Composition.Section<T>.Default.Get(@this);
	}

	extension(IServiceCollection @this)
	{
		public T? Section<T>(string name) where T : class
			=> @this.Configuration().Section<T>(name);

		public T? Section<T>() where T : class => @this.Configuration().Section<T>();
	}

	public static HostOperationsContext Operations(this BuildHostContext @this) => new(@this);

	extension(IServiceCollection @this)
	{
		public IConfigurationRoot ConfigurationRoot()
			=> @this.Configuration().To<IConfigurationRoot>();

		public IConfiguration Configuration()
			=> @this.Single(x => x.ServiceType == typeof(IConfiguration))
			        .ImplementationFactory?.Invoke(null!)
			        .To<IConfiguration>() ?? throw new InvalidOperationException();

		public ComponentRequest Component<T>()
		{
			var request = A.Type<T>();
			var result  = new ComponentRequest(request, @this.GetRequiredInstance<IComponentType>().Get(request));
			return result;
		}

		public Func<T> Deferred<T>() where T : class => new DeferredService<T>(@this).Get;

		public IServiceCollection Replace<T>(ServiceLifetime lifetime)
			where T : class
		{
			var existing = @this.FirstOrDefault(x => x.ServiceType == typeof(T));
			if (existing != null)
			{
				var instance = existing.ImplementationType != null
					               ? ServiceDescriptor.Describe(existing.ServiceType,
					                                            existing.ImplementationType,
					                                            lifetime)
					               : existing.ImplementationFactory != null
						               ? ServiceDescriptor.Describe(existing.ServiceType,
						                                            existing
							                                            .ImplementationFactory,
						                                            lifetime)
						               : null;
				if (instance != null)
				{
					@this.Replace(instance);
				}
			}

			return @this;
		}

		public T GetRequiredInstance<T>() where T : class
			=> (@this.Where(x => x.ServiceType == typeof(T))
			         .Select(x => x.ImplementationInstance)
			         .Only()
			    ??
			    @this.Select(x => x.ImplementationInstance)
			         .OfType<T>()
			         .FirstOrDefault()
			   )!
				.To<T>();

		public HostBuilderContext Context()
			=> @this.GetRequiredInstance<HostBuilderContext>();

		public string EnvironmentName()
			=> GetHostEnvironmentName.Default.Get(@this.Context());

		public StartRegistration<T> Start<T>() where T : class => new(@this);

		public IncludingRegistration ForDefinition<T>() where T : class
			=> new GenericDefinitionRegistration<T>(@this);
	}

	/**/

	public static IncludingRegistration Generic<T>(this StartRegistration<T> @this) where T : class
		=> new GenericDefinitionRegistration<T>(@this.Get());

	public static IServiceTypes Recursive(this Dependencies _) => RecursiveDependencies.Default;

	extension(BuildHostContext @this)
	{
		/**/
		public BuildHostContext WithComposition()
			=> Construction.WithComposition.Default.Get(@this);

		public BuildHostContext WithDefaultComposition()
			=> @this.Configure(Registrations.Default).ComposeUsing<ConfigureDefaultActivation>();

		public BuildHostContext WithDeferredRegistrations()
			=> @this.Configure(AddDeferredRegistrations.Default);

		public BuildHostContext RegisterModularity()
			=> @this.Configure(Composition.RegisterModularity.Default);

		public BuildHostContext WithPlatform(string platform)
			=> @this.Configure(new AssignHostPlatform(platform));
	}

	public static ICommand<IServiceCollection> Deferred(this ICommand<IServiceCollection> @this) => new Deferred(@this);

	public static BuildHostContext ConfigureFromEnvironment(this BuildHostContext @this)
		=> @this.Configure(Compose.ConfigureFromEnvironment.Default)
				.Configure(ConfigureHostBuilderFromEnvironment.Default);

	public static ICommand<IServiceCollection> ConfigureFromEnvironment(this ICommand<IServiceCollection> @this)
		=> Compose.ConfigureFromEnvironment.Default.Then().Append(@this).Get();

	extension(BuildHostContext @this)
	{
		public BuildHostContext ComposeUsingRoot<T>()
			where T : ICompositionRoot, new()
			=> @this.WithComposition().Configure(ConfigureContainer<T>.Default);

		public BuildHostContext ComposeUsing<T>()
			where T : class, ICommand<IServiceContainer>
			=> @this.ComposeUsing(DragonSpark.Compose.Start.An.Activation<T>().Activate());

		public BuildHostContext ComposeUsing(ICommand<IServiceContainer> configure)
			=> @this.ComposeUsing(configure.Execute);

		public BuildHostContext ComposeUsing(Action<IServiceContainer> configure)
			=> @this.WithComposition().Configure(new ConfigureContainer(configure));
	}
}