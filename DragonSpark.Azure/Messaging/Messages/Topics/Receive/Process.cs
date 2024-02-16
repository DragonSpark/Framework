using DragonSpark.Application.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

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