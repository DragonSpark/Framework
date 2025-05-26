using DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Messaging.Messages.Topics.Receive;

sealed class Process<T> : IStopAware<object>, IReportedTypeAware
{
	readonly IStopAware<T> _body;
	readonly Type          _reportedType;

	public Process(IStopAware<T> body) : this(body, body.GetType()) {}

	public Process(IStopAware<T> body, Type reportedType)
	{
		_body         = body;
		_reportedType = reportedType;
	}

	public ValueTask Get(Stop<object> parameter) => _body.Get(new(parameter.Subject.To<T>(), parameter));

	public Type Get() => _reportedType;
}