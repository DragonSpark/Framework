using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using DragonSpark.Operations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Octokit;
using Octokit.Internal;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Hosting.Server.GitHub
{
	public interface IProcessor : IOperation<EventMessage> {}

	sealed class Processor : IProcessor
	{
		readonly ILogger<Processor> _logger;
		readonly Array<IMessageHandler> _handlers;

		public Processor(ILogger<Processor> logger, Array<IMessageHandler> handlers)
		{
			_logger = logger;
			_handlers = handlers;
		}

		public async ValueTask Get(EventMessage parameter)
		{
			var length = _handlers.Length;
			for (var i = 0u; i < length; i++)
			{
				var handler = _handlers[i];
				if (handler.IsSatisfiedBy(parameter))
				{
					var task = handler.Get(parameter);

					if (task.IsFaulted)
					{
						_logger.LogError(task.AsTask().Exception, "[{Id}] An exception occurred while processing an event message of type {MessageType}.",
						                 parameter.Header.Delivery, parameter.Header.Event);
					}

					if (!task.IsCompleted)
					{
						await task.ConfigureAwait(false);
					}
				}
			}
		}
	}

	public interface IProcessorRegistration : ISelect<IServiceProvider, IProcessor> {}

	sealed class ProcessorRegistration : IProcessorRegistration
	{
		readonly Array<ISelect<IServiceProvider, IMessageHandler>> _registrations;

		public ProcessorRegistration(params ISelect<IServiceProvider, IMessageHandler>[] registrations)
			=> _registrations = registrations;

		public IProcessor Get(IServiceProvider parameter)
			=> new Processor(parameter.GetRequiredService<ILogger<Processor>>(),
			                 _registrations.Open().Introduce(parameter).Result());
	}

	public interface IHandlerRegistration<in T> : ISelect<IServiceProvider, IHandler<T>>
		where T : ActivityPayload {}

	sealed class HandlerRegistration<T, TPayload> : IHandlerRegistration<TPayload>
		where T : class, IHandler<TPayload>
		where TPayload : ActivityPayload
	{
		public static IHandlerRegistration<TPayload> Default { get; } = new HandlerRegistration<T, TPayload>();

		HandlerRegistration() : this(Is.Always()) {}

		readonly ICondition<TPayload> _condition;

		public HandlerRegistration(ICondition<TPayload> condition) => _condition = condition;

		public IHandler<TPayload> Get(IServiceProvider parameter)
			=> new ValidatedHandler<TPayload>(_condition, new LocatedHandler<T, TPayload>(parameter));
	}

	public interface IRegistrations : ISelect<IServiceProvider, IMessageHandler> {}

	public sealed class Registrations<T> : IRegistrations where T : ActivityPayload
	{
		readonly ICondition<EventMessage>                      _condition;
		readonly Array<ISelect<IServiceProvider, IHandler<T>>> _registrations;

		public Registrations(ICondition<EventMessage> condition,
		                     params ISelect<IServiceProvider, IHandler<T>>[] registrations)
			: this(condition, registrations.Result()) {}

		public Registrations(ICondition<EventMessage> condition,
		                     Array<ISelect<IServiceProvider, IHandler<T>>> registrations)
		{
			_condition     = condition;
			_registrations = registrations;
		}

		public IMessageHandler Get(IServiceProvider parameter)
			=> new MessageHandler<T>(_condition, _registrations.Open().Introduce(parameter).Result());
	}

	public interface IMessageHandler : IConditional<EventMessage, ValueTask> {}

	sealed class MessageHandler<T> : ConditionAware<EventMessage>, IMessageHandler where T : ActivityPayload
	{
		readonly Func<EventMessage, T> _deserializer;
		readonly Array<IHandler<T>>    _entries;

		public MessageHandler(ICondition<EventMessage> candidate, Array<IHandler<T>> entries)
			: this(EventMessages<T>.Default.Get, candidate, entries) {}

		public MessageHandler(Func<EventMessage, T> deserializer, ICondition<EventMessage> candidate,
		                      Array<IHandler<T>> entries) : base(candidate)
		{
			_deserializer = deserializer;
			_entries      = entries;
		}

		public async ValueTask Get(EventMessage parameter)
		{
			var payload = _deserializer(parameter);
			var length  = _entries.Length;
			for (var i = 0u; i < length; i++)
			{
				var task = _entries[i].Get(payload);
				if (!task.IsCompleted)
				{
					await task;
				}
			}
		}
	}

	public interface IHandler<in T> : IOperation<T> where T : ActivityPayload {}

	sealed class ValidatedHandler<T> : Validated<T, ValueTask>, IHandler<T> where T : ActivityPayload
	{
		public ValidatedHandler(ICondition<T> condition, ISelect<T, ValueTask> handler)
			: base(condition, handler, Compose.Start.A.Selection<T>()
			                                  .By.Calling(_ => new ValueTask(Task.CompletedTask))) {}
	}

	sealed class LocatedHandler<T, TPayload> : Select<TPayload, ValueTask> where T : class, IHandler<TPayload>
	                                                                       where TPayload : ActivityPayload
	{
		public LocatedHandler(IServiceProvider locator) : base(Compose.Start.A.Result<IHandler<TPayload>>()
		                                                              .By.Calling(locator.GetRequiredService<T>)
		                                                              .Assume()) {}
	}

	sealed class Serializer : Instance<IJsonSerializer>, IJsonSerializer
	{
		public static Serializer Default { get; } = new Serializer();

		Serializer() : base(new SimpleJsonSerializer()) {}

		public string Serialize(object item) => Get().Serialize(item);

		public T Deserialize<T>(string json) => Get().Deserialize<T>(json);
	}
}
