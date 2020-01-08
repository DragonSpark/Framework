using DragonSpark.Compose;
using DragonSpark.Composition;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using Flurl;
using JetBrains.Annotations;
using LaunchDarkly.EventSource;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DragonSpark.Application.Hosting.Server.GitHub.Environment
{
	public sealed class ServiceConfiguration : Command<IServiceCollection>, IServiceConfiguration
	{
		[UsedImplicitly]
		public static ServiceConfiguration Default { get; } = new ServiceConfiguration();

		ServiceConfiguration() : base(Start.An.Option<RelayerSettings>()
		                                   .Then()
		                                   .Terminate(Registrations.Default)) {}
	}

	sealed class Registrations : ICommand<IServiceCollection>
	{
		public static Registrations Default { get; } = new Registrations();

		Registrations() {}

		public void Execute(IServiceCollection parameter)
		{
			parameter.AddHttpClient<Destination>()
			         .Return(parameter)
			         //
			         .For<IEventSource>()
			         .Use<Relayer>()
			         .Singleton()
			         //
			         .For<RelayOperation>()
			         .Use<RelayOperations>()
			         .Singleton()
			         //
			         .AddSingleton<RelayRegistration>()
			         .AddSingleton<RegisteredSource>();
		}
	}

	public sealed class RelayerSettings
	{
		public Uri Source { get; set; }

		public Uri Destination { get; set; }
	}

	sealed class Destination : ISelect<MessageEvent, Task>
	{
		readonly ILogger    _logger;
		readonly HttpClient _client;
		readonly Uri        _location;

		[UsedImplicitly]
		public Destination(ILogger<Relayer> logger, HttpClient client, RelayerSettings settings)
			: this(logger, client, settings.Destination) {}

		Destination(ILogger<Relayer> logger, HttpClient client, Uri location)
		{
			_logger   = logger;
			_client   = client;
			_location = location;
		}

		public async Task Get(MessageEvent parameter)
		{
			var message = JsonConvert.DeserializeObject<Message>(parameter.Data);

			var url = new Url(_location).SetQueryParams(message.Query).ToUri();

			var body    = message.Body.ToString(Formatting.None);
			var content = new StringContent(body);

			foreach (var (key, value) in message.Properties)
			{
				content.Headers.TryAddWithoutValidation(key, value.ToString());
			}

			try
			{
				var response = await _client.PostAsync(url, content);
				response.EnsureSuccessStatusCode();
				_logger.LogInformation($"{response.RequestMessage.Method} ${response.RequestMessage.RequestUri} - ${response.StatusCode:D}");
			}
			catch (HttpRequestException e)
			{
				_logger.LogError(e, "Error encountered while sending data to destination.");
			}
		}
	}

	[JsonObject(MemberSerialization.OptOut)]
	sealed class Message
	{
		public JObject Query { get; set; }

		public JObject Body { get; set; }

		[JsonExtensionData]
		public IDictionary<string, JToken> Properties { get; } = new Dictionary<string, JToken>();
	}

	sealed class CloseAwareEventSource : IEventSource
	{
		readonly IEventSource _source;
		readonly Action       _close;

		public CloseAwareEventSource(IEventSource source, Action close)
		{
			_source = source;
			_close  = close;
		}

		public Task StartAsync() => _source.StartAsync();

		public void Close()
		{
			_close();
			_source.Close();
		}

		public ReadyState ReadyState => _source.ReadyState;

		public event EventHandler<StateChangedEventArgs> Opened
		{
			add => _source.Opened += value;
			remove => _source.Opened -= value;
		}

		public event EventHandler<StateChangedEventArgs> Closed
		{
			add => _source.Closed += value;
			remove => _source.Closed -= value;
		}

		public event EventHandler<MessageReceivedEventArgs> MessageReceived
		{
			add => _source.MessageReceived += value;
			remove => _source.MessageReceived -= value;
		}

		public event EventHandler<CommentReceivedEventArgs> CommentReceived
		{
			add => _source.CommentReceived += value;
			remove => _source.CommentReceived -= value;
		}

		public event EventHandler<ExceptionEventArgs> Error
		{
			add => _source.Error += value;
			remove => _source.Error -= value;
		}
	}

	sealed class RegisterRelay : ICommand<IEventSource>
	{
		readonly EventHandler<StateChangedEventArgs>    _open;
		readonly EventHandler<MessageReceivedEventArgs> _message;
		readonly EventHandler<ExceptionEventArgs>       _error;

		public RegisterRelay(RelayOperation handler) : this(handler.Open, handler.Message, handler.Error) {}

		public RegisterRelay(EventHandler<StateChangedEventArgs> open, EventHandler<MessageReceivedEventArgs> message,
		                     EventHandler<ExceptionEventArgs> error)
		{
			_open    = open;
			_message = message;
			_error   = error;
		}

		public void Execute(IEventSource parameter)
		{
			parameter.Opened          += _open;
			parameter.MessageReceived += _message;
			parameter.Error           += _error;
		}
	}

	sealed class RelayOperation
	{
		public RelayOperation(EventHandler<StateChangedEventArgs> open, EventHandler<MessageReceivedEventArgs> message,
		                      EventHandler<ExceptionEventArgs> error)
		{
			Open    = open;
			Message = message;
			Error   = error;
		}

		public EventHandler<StateChangedEventArgs> Open { get; }

		public EventHandler<MessageReceivedEventArgs> Message { get; }

		public EventHandler<ExceptionEventArgs> Error { get; }
	}

	sealed class CancelRelay : ICommand<IEventSource>
	{
		readonly EventHandler<StateChangedEventArgs>    _open;
		readonly EventHandler<MessageReceivedEventArgs> _message;
		readonly EventHandler<ExceptionEventArgs>       _error;

		public CancelRelay(RelayOperation handler) : this(handler.Open, handler.Message, handler.Error) {}

		public CancelRelay(EventHandler<StateChangedEventArgs> open, EventHandler<MessageReceivedEventArgs> message,
		                   EventHandler<ExceptionEventArgs> error)
		{
			_open    = open;
			_message = message;
			_error   = error;
		}

		public void Execute(IEventSource parameter)
		{
			parameter.Opened          -= _open;
			parameter.MessageReceived -= _message;
			parameter.Error           -= _error;
		}
	}

	sealed class RelayOperations : IResult<RelayOperation>
	{
		readonly ILogger     _logger;
		readonly Uri         _source;
		readonly Destination _destination;

		[UsedImplicitly]
		public RelayOperations(ILogger<Relayer> logger, RelayerSettings settings, Destination destination)
			: this(logger, settings.Source, destination) {}

		public RelayOperations(ILogger<Relayer> logger, Uri source, Destination destination)
		{
			_logger      = logger;
			_source      = source;
			_destination = destination;
		}

		void OnOpen(object sender, StateChangedEventArgs e)
		{
			_logger.LogInformation("Connected. {Uri}", _source);
		}

		void OnMessage(object sender, MessageReceivedEventArgs eventArgs)
		{
			switch (eventArgs.EventName)
			{
				case "ready":
				case "ping":
					break;
				default:
					_destination?.Get(eventArgs.Message);
					break;
			}
		}

		void OnError(object sender, ExceptionEventArgs e)
		{
			_logger.LogError(e.Exception, "An exception has occurred.");
		}

		public RelayOperation Get() => new RelayOperation(OnOpen, OnMessage, OnError);
	}

	sealed class RelayRegistration
	{
		public RelayRegistration(RelayOperation operation)
			: this(new RegisterRelay(operation), new CancelRelay(operation)) {}

		public RelayRegistration(ICommand<IEventSource> register, ICommand<IEventSource> cancel)
		{
			Register = register;
			Cancel   = cancel;
		}

		public ICommand<IEventSource> Register { get; }

		public ICommand<IEventSource> Cancel { get; }
	}

	sealed class RegisteredSource : ISelect<Configuration, IEventSource>
	{
		readonly ILogger<Relayer>  _logger;
		readonly RelayRegistration _registration;
		readonly Uri               _destination;

		[UsedImplicitly]
		public RegisteredSource(ILogger<Relayer> logger, RelayRegistration registration, RelayerSettings settings)
			: this(logger, registration, settings.Destination) {}

		public RegisteredSource(ILogger<Relayer> logger, RelayRegistration registration, Uri destination)
		{
			_logger       = logger;
			_registration = registration;
			_destination  = destination;
		}

		public IEventSource Get(Configuration parameter)
		{
			var source = new EventSource(parameter);

			_logger.LogInformation("Forwarding {From} to {To}", parameter.Uri, _destination);

			_registration.Register.Execute(source);

			var result = new CloseAwareEventSource(source, _registration.Cancel.Then().Bind(source));
			return result;
		}
	}

	sealed class Relayer : FixedSelection<Configuration, IEventSource>
	{
		[UsedImplicitly]
		public Relayer(RegisteredSource source, RelayerSettings settings)
			: base(source,
			       new Configuration(settings.Source, delayRetryDuration: TimeSpan.Zero)) {}
	}
}