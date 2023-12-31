using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Azure.Events;

public class EventRegistration<T, U> : EventRegistration<T> where T : Message<U>
{
	protected EventRegistration(IOperation<U> body, IExceptionLogger logger)
		: base(Start.A.Selection<T>().By.Calling(x => x.Subject).Select(body).Out(), logger) {}
}

public class EventRegistration<T> : IEventRegistration where T : class
{
	readonly string             _key;
	readonly IOperation<object> _body;

	protected EventRegistration(IOperation<T> body, IExceptionLogger logger)
		: this(A.Type<T>().FullName.Verify(),
		       new ExceptionLoggingAware<object>(Start.A.Selection<object>().By.Cast<T>().Select(body).Out(), logger,
		                                         A.Type<EventRegistration<T>>())) {}

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
