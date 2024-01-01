using DragonSpark.Application.Connections.Client;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
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
	readonly ITable<string, RegistryEntry> _registry;
	readonly string                        _key;

	protected Subscribe() : this(Entries.Default, A.Type<T>().FullName.Verify()) {}

	protected Subscribe(ITable<string, RegistryEntry> registry, string key)
	{
		_registry = registry;
		_key      = key;
	}

	public ISubscription Get(Func<T, Task> parameter)
	{
		var entry = _registry.TryGet(_key, out var current)
			            ? current
			            : _registry.Parameter(new(_key, new RegistryEntry(A.Type<T>()))).Value;
		var operation = new Process<T>(parameter.Start().Out().Then().Structure().Out(),
		                               parameter.Target.Verify().GetType());
		return new Subscription(entry.Handlers, operation);
	}
}

public class Subscribe<T, U> : ISubscribe<T> where U : Message<T>
{
	readonly ITable<string, RegistryEntry> _registry;
	readonly string                        _key;

	protected Subscribe() : this(Entries.Default, A.Type<U>().FullName.Verify()) {}

	protected Subscribe(ITable<string, RegistryEntry> registry, string key)
	{
		_registry = registry;
		_key      = key;
	}

	public ISubscription Get(Func<T, Task> parameter)
	{
		var entry = _registry.TryGet(_key, out var current)
			            ? current
			            : _registry.Parameter(new(_key, new RegistryEntry(A.Type<U>()))).Value;
		var body = Start.A.Selection<U>().By.Calling(x => x.Subject).Select(parameter).Out().Then().Structure().Out();
		var operation = new Process<U>(body, parameter.Target.Verify().GetType());
		return new Subscription(entry.Handlers, operation);
	}
}

/*public interface ISubscription : IOperation, IAsyncDisposable;*/

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