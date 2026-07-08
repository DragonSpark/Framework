using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using DragonSpark.Model.Operations.Stop;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;

namespace DragonSpark.Application.AspNet.Communication.Http.Diagnostics;

public class PolicyAwareClientRequest : PolicyAwareClientRequest<Guid>
{
	protected PolicyAwareClientRequest(IStopAware<Guid> previous, ILogger logger, string message)
		: base(previous, logger, message) {}

	protected PolicyAwareClientRequest(IStopAware<Guid> previous, ILogException<Guid> template)
		: base(previous, template) {}
}

public class PolicyAwareClientRequest<TIn, T> : StopAware<TIn>
{
	protected PolicyAwareClientRequest(IStopAware<TIn> previous, Func<TIn, T> select, ILogException<T> template)
		: base(previous.Then().Use(template).Calling(select).When<HttpRequestException>().Get()) {}
}

public class PolicyAwareClientRequest<T> : StopAware<T>
{
	protected PolicyAwareClientRequest(IStopAware<T> previous, ILogger logger, string message)
		: this(previous, new LogWarningException<T>(logger, message)) {}

	protected PolicyAwareClientRequest(IStopAware<T> previous, ILogException<T> template)
		: base(previous.Then().UsePolicy(template).When<HttpRequestException>()) {}
}