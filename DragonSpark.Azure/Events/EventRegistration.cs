using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Stores;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Events;

public class EventRegistration<T, U> : EventRegistration<T> where T : Message<U>
{
	protected EventRegistration(IOperation<U> body)
		: base(Start.A.Selection<T>().By.Calling(x => x.Subject).Select(body).Out()) {}
}

public class EventRegistration<T> : IEventRegistration where T : class
{
	readonly IKeyedEntry        _entry;
	readonly EntryKey           _key;
	readonly IOperation<object> _body;

	protected EventRegistration(IOperation<T> body)
		: this(KeyedEntry<T>.Default, new(A.Type<T>().FullName.Verify()), new Process<T>(body)) {}

	protected EventRegistration(IKeyedEntry entry, EntryKey key, IOperation<object> body)
	{
		_entry = entry;
		_key   = key;
		_body  = body;
	}

	public void Execute(ITable<EntryKey, RegistryEntry> parameter)
	{
		var entry = _entry.Get(_key);
		entry.Handlers.Add(_body);
	}
}

sealed class Process<T> : IOperation<object>, IReportedTypeAware
{
	readonly IOperation<T> _body;
	readonly Type          _reportedType;

	public Process(IOperation<T> body) : this(body, body.GetType()) {}

	public Process(IOperation<T> body, Type reportedType)
	{
		_body         = body;
		_reportedType = reportedType;
	}

	public ValueTask Get(object parameter) => _body.Get(parameter.To<T>());

	public Type Get() => _reportedType;
}