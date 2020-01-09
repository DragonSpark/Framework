using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences;
using Microsoft.Extensions.DependencyInjection;
using Octokit;
using Octokit.Internal;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Hosting.Server.GitHub
{
	public interface IProcessor : IOperation<EventMessage> {}

	sealed class Processor : IProcessor
	{
		readonly Array<IMessageHandler> _handlers;

		public Processor(Array<IMessageHandler> handlers) => _handlers = handlers;

		public async ValueTask Get(EventMessage parameter)
		{
			var length = _handlers.Length;
			for (var i = 0u; i < length; i++)
			{
				var handler = _handlers[i];
				if (handler.IsSatisfiedBy(parameter))
				{
					var task = handler.Get(parameter);
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
			=> new Processor(_registrations.Open().Introduce(parameter).Result());
	}

	public interface IHandlerRegistration<in T> : ISelect<IServiceProvider, IHandler<T>>
		where T : ActivityPayload {}

	sealed class HandlerRegistration<T, TPayload> : IHandlerRegistration<TPayload>
		where T : class, IHandler<TPayload>
		where TPayload : ActivityPayload
	{
		public static IHandlerRegistration<TPayload> Default { get; } = new HandlerRegistration<T, TPayload>();

		HandlerRegistration() : this(Is.Always().Out()) {}

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
		{
			var template  = parameter.GetRequiredService<FaultAwareTemplate<T>>();
			var operation = new MessageOperation<T>(_condition, _registrations.Open().Introduce(parameter).Result());
			var result = new MessageHandler(operation.Condition,
			                                operation.Then()
			                                         .Bind(template)
			                                         .WithArguments(x => (x.Header.Delivery, x.Header.Event))
			                                         .Get);
			return result;
		}
	}

	public interface IMessageHandler : IConditional<EventMessage, ValueTask> {}

	sealed class MessageHandler : ConditionAware<EventMessage>, IMessageHandler
	{
		readonly Func<EventMessage, ValueTask> _select;

		public MessageHandler(ICondition<EventMessage> condition, Func<EventMessage, ValueTask> select)
			: base(condition) => _select = select;

		public ValueTask Get(EventMessage parameter) => _select(parameter);
	}

	sealed class MessageOperation<T> : ConditionAware<EventMessage>, IOperation<EventMessage> where T : ActivityPayload
	{
		readonly Func<EventMessage, T> _deserializer;
		readonly Array<IHandler<T>>    _entries;

		public MessageOperation(ICondition<EventMessage> candidate, Array<IHandler<T>> entries)
			: this(EventMessages<T>.Default.Get, candidate, entries) {}

		public MessageOperation(Func<EventMessage, T> deserializer, ICondition<EventMessage> candidate,
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
					await task.ConfigureAwait(false);
				}
			}
		}
	}

	public interface IHandler<in T> : IOperation<T> where T : ActivityPayload {}

	sealed class ValidatedHandler<T> : Validated<T, ValueTask>, IHandler<T> where T : ActivityPayload
	{
		public ValidatedHandler(ICondition<T> condition, ISelect<T, ValueTask> handler)
			: base(condition.Get, handler.Get, Start.A.Selection<T>()
			                                        .By.Calling(_ => new ValueTask(Task.CompletedTask))) {}
	}

	sealed class LocatedHandler<T, TPayload> : Select<TPayload, ValueTask> where T : class, IHandler<TPayload>
	                                                                       where TPayload : ActivityPayload
	{
		public LocatedHandler(IServiceProvider locator)
			: base(Start.A.Result<IHandler<TPayload>>()
			            .By.Calling(locator.GetRequiredService<T>)
			            .Get()
			            .To(A.SelectionResult)
			            .Then()
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