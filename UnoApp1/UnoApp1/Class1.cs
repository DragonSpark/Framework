using DragonSpark.Application;
using DragonSpark.Application.Mobile;
using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Selection;
using JetBrains.Annotations;
using Uno.Resizetizer;

namespace UnoApp1;

class Class1;

// TODO

sealed class RunApplication : DragonSpark.Application.Hosting.Uno.Run.RunApplication // TODO: Handle exception
{
	public static RunApplication Default { get; } = new();

	RunApplication() : base(NewHost.Default, ConfigureBuilder.Default.Execute, ApplicationHost.Default.Get) {}
}

sealed class ApplicationHost : IAllocating<IApplicationBuilder, IHost>
{
	public static ApplicationHost Default { get; } = new();

	ApplicationHost() : this(SelectNavigation.Default) {}

	readonly ISelectNavigation _select;

	public ApplicationHost(ISelectNavigation select) => _select = select;

	[MustDisposeResource(false)]
	public Task<IHost> Get(IApplicationBuilder parameter) => parameter.NavigateAsync<Shell>(_select.Get(parameter));
}

public interface ISelectNavigation : ISelect<IApplicationBuilder, Func<IServiceProvider, INavigator, Task>>;

sealed class SelectNavigation(string nested) : ISelectNavigation
{
	public static SelectNavigation Default { get; } = new();

	SelectNavigation() : this(Qualifiers.Nested) {}

	readonly string _nested = nested;

	public Func<IServiceProvider, INavigator, Task> Get(IApplicationBuilder parameter)
		=> async (services, navigator) =>
		   {
			   await navigator.NavigateViewModelAsync<MainModel>(parameter.App, _nested).Await();
		   };
}

sealed class NewHost : DragonSpark.Application.Hosting.Uno.Run.NewHost
{
	public static NewHost Default { get; } = new();

	NewHost()
		: base(Start.A.Host()
					.WithDefaultComposition()
					.RegisterModularity()
					.WithDeferredRegistrations()
					.WithAmbientConfiguration()
					.WithFrameworkConfigurations()
					//
					.WithInitializationLogging<RunApplication>()
					.Configure(ConfigureHostBuilder.Default)) {}
}

sealed class ConfigureBuilder : ICommand<IApplicationBuilder>
{
	public static ConfigureBuilder Default { get; } = new();

	ConfigureBuilder() {}

	public void Execute(IApplicationBuilder parameter)
	{
		parameter.UseToolkitNavigation();

		// TODO: Environmental:
#if DEBUG
		parameter.Window.UseStudio();
#endif
		parameter.Window.SetWindowIcon();
	}
}


sealed class ConfigureHostBuilder : ICommand<IHostBuilder>
{
	public static ConfigureHostBuilder Default { get; } = new();

	ConfigureHostBuilder() {}

	public void Execute(IHostBuilder parameter)
	{
		parameter
			// TODO: Environmental:
#if DEBUG
			// Switch to Development environment when running in DEBUG
			.UseEnvironment(Environments.Development)
#endif
			.UseLogging((context, logBuilder) =>
						{
							// Configure log levels for different categories of logging
							logBuilder.SetMinimumLevel(context.HostingEnvironment
															  .IsDevelopment()
														   ? LogLevel.Information
														   : LogLevel.Warning)

									  // Default filters for core Uno Platform namespaces
									  .CoreLogLevel(LogLevel.Warning);

							// Uno Platform namespace filter groups
							// Uncomment individual methods to see more detailed logging
							//// Generic Xaml events
							//logBuilder.XamlLogLevel(LogLevel.Debug);
							//// Layout specific messages
							//logBuilder.XamlLayoutLogLevel(LogLevel.Debug);
							//// Storage messages
							//logBuilder.StorageLogLevel(LogLevel.Debug);
							//// Binding related messages
							//logBuilder.XamlBindingLogLevel(LogLevel.Debug);
							//// Binder memory references tracking
							//logBuilder.BinderMemoryReferenceLogLevel(LogLevel.Debug);
							//// DevServer and HotReload related
							//logBuilder.HotReloadCoreLogLevel(LogLevel.Information);
							//// Debug JS interop
							//logBuilder.WebAssemblyLogLevel(LogLevel.Debug);
						}, enableUnoLogging: true)
			.UseConfiguration(configure: y => y.EmbeddedSource<App>().Section<AppConfig>())
			// Enable localization (see appsettings.json for supported languages)
			.UseLocalization()
			// Register Json serializers (ISerializer and ISerializer)
			.UseHttp((context, services)
						 => services // Register HttpClient
						    // TODO: Environmental:
#if DEBUG
							// DelegatingHandler will be automatically injected into Refit Client
							.AddTransient<DelegatingHandler, DebugHttpHandler>()
#endif
							.AddSingleton<IWeatherCache, WeatherCache>()
							.AddRefitClient<IApiClient>(context))
			.ConfigureServices((_, _) =>
							   {
								   // TODO: Register your services
								   //services.AddSingleton<IMyService, MyService>();
							   })
			.UseNavigation(ReactiveViewModelMappings.ViewModelMappings, RegisterRoutes);
	}

	static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
	{
		views.Register(new(ViewModel: typeof(ShellModel)),
					   new ViewMap<MainPage, MainModel>(),
					   new DataViewMap<SecondPage, SecondModel, Entity>());

		routes.Register(new RouteMap(string.Empty, View: views.FindByViewModel<ShellModel>(),
									 Nested:
									 [
										 new("Main", View: views.FindByViewModel<MainModel>(), IsDefault: true),
										 new("Second", View: views.FindByViewModel<SecondModel>()),
									 ])
					   );
	}
}
