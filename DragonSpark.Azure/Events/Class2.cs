using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using NetFabric.Hyperlinq;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events;

public interface IEntries : ITable<string, RegistryEntry>, ICommand;

sealed class Entries : ConcurrentTable<string, RegistryEntry>, IEntries
{
	public static Entries Default { get; } = new();

	Entries() {}
}

public sealed record RegistryEntry(Type Key, ConcurrentBag<IOperation<object>> Handlers)
{
	public RegistryEntry(Type Key) : this(Key, new()) {}
}

sealed class HandleEvent : IOperation<ProcessEventArgs>
{
	public static HandleEvent Default { get; } = new();

	HandleEvent() : this(GetEntry.Default) {}

	readonly ISelect<EventData, RegistryEntry?> _entry;

	public HandleEvent(ISelect<EventData, RegistryEntry?> entry) => _entry = entry;

	public async ValueTask Get(ProcessEventArgs parameter)
	{
		var entry = _entry.Get(parameter.Data);
		if (entry is not null)
		{
			var (key, handlers) = entry;
			var message = await parameter.Data.Data.Verify().ToObjectAsync(key).ConfigureAwait(false);
			if (message is not null)
			{
				using var lease = handlers.AsValueEnumerable().ToArray(ArrayPool<IOperation<object>>.Shared);
				foreach (var operation in lease)
				{
					await operation.Await(message);
				}
			}
		}
	}
}

sealed class GetEntry : ISelect<EventData, RegistryEntry?>
{
	public static GetEntry Default { get; } = new();

	GetEntry() : this(Entries.Default) {}

	readonly ITable<string, RegistryEntry> _registry;

	public GetEntry(ITable<string, RegistryEntry> registry) => _registry = registry;

	public RegistryEntry? Get(EventData parameter)
		=> parameter.Properties.TryGetValue(EventType.Default, out var type)
		   &&
		   _registry.TryGet(type.To<string>(), out var element)
			   ? element
			   : null;
}

public interface IEventRegistration : ICommand<ITable<string, RegistryEntry>>;

public class UserEventRegistration<T> : EventRegistration<T, uint> where T : UserMessage
{
	protected UserEventRegistration(IOperation<uint> body, IExceptionLogger logger) : base(body, logger) {}
}

public class EventRegistration<T, U> : EventRegistration<T> where T : Message<U>
{
	protected EventRegistration(IOperation<U> body, IExceptionLogger logger)
		: base(Start.A.Selection<T>().By.Calling(x => x.Subject).Select(body).Out(), logger) {}
}

public record UserMessage(uint Subject) : Message<uint>(Subject);

public record Message<T>(T Subject);

public class EventRegistration<T> : IEventRegistration where T : class
{
	readonly string             _key;
	readonly IOperation<object> _body;

	protected EventRegistration(IOperation<T> body, IExceptionLogger logger)
		: this(A.Type<T>().FullName.Verify(),
		       new ExceptionLoggingAware<object>(Start.A.Selection<object>().By.Cast<T>().Select(body).Out(), logger,
		                                         A.Type<EventRegistration<T>>())
		      ) {}

	protected EventRegistration(string key, IOperation<object> body)
	{
		_key  = key;
		_body = body;
	}

	public void Execute(ITable<string, RegistryEntry> parameter)
	{
		var entry = parameter.TryGet(_key, out var current)
			            ? current
			            : parameter.Parameter(new(_key, new RegistryEntry(A.Type<T>()))).Value;
		entry.Handlers.Add(_body);
	}
}

/*public class SubscriptionEntry<T> : Instance<ISubscription>, ISubscriptionEntry
{
	protected SubscriptionEntry(ISubscribe<T> subscribe, IOperation<T> on) : this(subscribe, on.Then().Allocate()) {}

	protected SubscriptionEntry(ISubscribe<T> subscribe, Func<T, Task> on) : base(subscribe.Get(on)) {}
}

sealed class HubSubscriptions : IArray<ISubscription>
{
	readonly IEnumerable<ISubscriptionEntry> _subscriptions;

	public HubSubscriptions(IEnumerable<ISubscriptionEntry> subscriptions) => _subscriptions = subscriptions;

	public Array<ISubscription> Get() => _subscriptions.Select(x => x.Get()).ToArray();
}

public class Subscribe : ISubscribe
{
	readonly IResult<HubConnection> _connection;
	readonly string                 _name;

	protected Subscribe(IResult<HubConnection> connection, string name)
	{
		_connection = connection;
		_name       = name;
	}

	public ISubscription Get(Func<Task> parameter)
		=> new PolicyAwareSubscription(new Subscription(_connection, _name, parameter));
}

public class Subscribe<T> : ISubscribe<T>
{
	readonly IResult<HubConnection> _connection;
	readonly string                 _name;

	protected Subscribe(IResult<HubConnection> connection, string name)
	{
		_connection = connection;
		_name       = name;
	}

	public ISubscription Get(Func<T, Task> parameter)
		=> new PolicyAwareSubscription(new Subscription<T>(_connection, _name, parameter));
}

sealed class PolicyAwareSubscription : PolicyAwareOperation, ISubscription
{
	readonly ISubscription _previous;

	public PolicyAwareSubscription(ISubscription previous) : base(previous, ExtendedDurableConnectionPolicy.Default)
		=> _previous = previous;

	public ValueTask DisposeAsync() => _previous.DisposeAsync();
}*/