using DragonSpark.Composition;
using DragonSpark.Grok.Chat;
using DragonSpark.Model.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace DragonSpark.Grok;

sealed class Registrations : ICommand<IServiceCollection>
{
	public static Registrations Default { get; } = new();

	Registrations() : this(RegistrationName.Default, ClientConfiguration.Default.Execute) {}

	readonly string                               _name;
	readonly Action<IServiceProvider, HttpClient> _configure;

	public Registrations(string name, Action<IServiceProvider, HttpClient> configure)
	{
		_name      = name;
		_configure = configure;
	}

	public void Execute(IServiceCollection parameter)
	{
		parameter.Register<GrokApiSettings>()
		         //
		         .Start<ConfigureClient>()
		         .Singleton()
		         //
		         .Then.AddHttpClient(_name, _configure)
		         .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
		         {
			         PooledConnectionLifetime = TimeSpan.FromMinutes(10)
		         });
	}
}