using DragonSpark.Application.Connections.Client;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events;

public record Message<T>(T Subject);

// public interface ISubscribe : ISelect<Func<Task>, ISubscription>;

//public interface ISubscribe<out T> : ISelect<Func<T, Task>, ISubscription>;

public class Subscribe<T> : ISubscribe<T>
{
	readonly IKeyedEntry                                _entry;
	readonly EntryKey                                   _key;
	readonly ISelect<Func<T, Task>, IOperation<object>> _operation;

	protected Subscribe()
		: this(KeyedEntry<T>.Default, new(A.Type<T>().FullName.Verify()), CreateOperation<T>.Default) {}

	protected Subscribe(IKeyedEntry entry, EntryKey key, ISelect<Func<T, Task>, IOperation<object>> operation)
	{
		_entry     = entry;
		_key       = key;
		_operation = operation;
	}

	public ISubscription Get(Func<T, Task> parameter)
	{
		var (_, handlers) = _entry.Get(_key);
		var operation = _operation.Get(parameter);
		return new Subscription(handlers, operation);
	}
}

public class Subscriber<T> : ISubscriber<T>
{
	readonly IKeyedEntry                                _entry;
	readonly ISelect<Func<T, Task>, IOperation<object>> _operation;
	readonly string                                     _key;

	protected Subscriber() : this(KeyedEntry<T>.Default, A.Type<T>().FullName.Verify(), CreateOperation<T>.Default) {}

	public Subscriber(IKeyedEntry entry, string key, ISelect<Func<T, Task>, IOperation<object>> operation)
	{
		_entry     = entry;
		_operation = operation;
		_key       = key;
	}

	public ISubscription Get(SubscriberInput<T> parameter)
	{
		var (recipient, subject) = parameter;
		var (_, handlers)        = _entry.Get(new(recipient, _key));
		var operation = _operation.Get(subject);
		return new Subscription(handlers, operation);
	}
}

public class Subscriber<T, U> : ISubscriber<T> where U : Message<T>
{
	readonly IKeyedEntry                                _entry;
	readonly string                                     _key;
	readonly ISelect<Func<T, Task>, IOperation<object>> _operation;

	protected Subscriber()
		: this(KeyedEntry<U>.Default, A.Type<U>().FullName.Verify(), CreateOperation<T, U>.Default) {}

	protected Subscriber(IKeyedEntry entry, string key, ISelect<Func<T, Task>, IOperation<object>> operation)
	{
		_entry     = entry;
		_operation = operation;
		_key       = key;
	}

	public ISubscription Get(SubscriberInput<T> parameter)
	{
		var (recipient, subject) = parameter;
		var (_, handlers)        = _entry.Get(new(recipient, _key));
		var operation = _operation.Get(subject);
		return new Subscription(handlers, operation);
	}
}

public class Subscribe<T, U> : ISubscribe<T> where U : Message<T>
{
	readonly IKeyedEntry                                _entry;
	readonly EntryKey                                   _key;
	readonly ISelect<Func<T, Task>, IOperation<object>> _operation;

	protected Subscribe()
		: this(KeyedEntry<U>.Default, new(A.Type<U>().FullName.Verify()), CreateOperation<T, U>.Default) {}

	protected Subscribe(IKeyedEntry entry, EntryKey key, ISelect<Func<T, Task>, IOperation<object>> operation)
	{
		_entry     = entry;
		_key       = key;
		_operation = operation;
	}

	public ISubscription Get(Func<T, Task> parameter)
	{
		var (_, handlers) = _entry.Get(_key);
		var operation = _operation.Get(parameter);
		return new Subscription(handlers, operation);
	}
}

sealed class CreateOperation<T> : ISelect<Func<T, Task>, IOperation<object>>
{
	public static CreateOperation<T> Default { get; } = new();

	CreateOperation() {}

	public IOperation<object> Get(Func<T, Task> parameter)
		=> new Process<T>(parameter.Start().Out().Then().Structure().Out(), parameter.Target.Verify().GetType());
}

sealed class CreateOperation<T, U> : ISelect<Func<T, Task>, IOperation<object>> where U : Message<T>
{
	public static CreateOperation<T, U> Default { get; } = new();

	CreateOperation() {}

	public IOperation<object> Get(Func<T, Task> parameter)
	{
		var body   = Start.A.Selection<U>().By.Calling(x => x.Subject).Select(parameter).Out().Then().Structure().Out();
		var result = new Process<U>(body, parameter.Target.Verify().GetType());
		return result;
	}
}

public interface IKeyedEntry : ISelect<EntryKey, RegistryEntry>;

public sealed class KeyedEntry<T> : KeyedEntry
{
	public static KeyedEntry<T> Default { get; } = new();

	KeyedEntry() : base(_ => new(A.Type<T>())) {}
}

public class KeyedEntry : IKeyedEntry
{
	readonly ITable<EntryKey, RegistryEntry> _registry;
	readonly Func<EntryKey, RegistryEntry>   _new;

	protected KeyedEntry(Func<EntryKey, RegistryEntry> @new) : this(Entries.Default, @new) {}

	protected KeyedEntry(ITable<EntryKey, RegistryEntry> registry, Func<EntryKey, RegistryEntry> @new)
	{
		_registry = registry;
		_new      = @new;
	}

	public RegistryEntry Get(EntryKey parameter)
		=> _registry.TryGet(parameter, out var current)
			   ? current
			   : _registry.Parameter(new(parameter, _new(parameter))).Value;
}

sealed class Subscription : ISubscription
{
	readonly SynchronizedCollection<IOperation<object>> _handlers;
	readonly IOperation<object>                         _subject;

	public Subscription(SynchronizedCollection<IOperation<object>> handlers, IOperation<object> subject)
	{
		_handlers = handlers;
		_subject  = subject;
	}

	public ValueTask DisposeAsync()
	{
		_handlers.Remove(_subject);
		return ValueTask.CompletedTask;
	}

	public ValueTask Get()
	{
		if (!_handlers.Contains(_subject))
		{
			_handlers.Add(_subject);
		}

		return ValueTask.CompletedTask;
	}
}