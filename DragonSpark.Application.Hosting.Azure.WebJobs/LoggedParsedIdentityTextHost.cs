using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated.Stop;
using DragonSpark.Model.Operations.Stop;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public class LoggedParsedIdentityTextHost : IAllocated<string>
{
	readonly IAllocated<string> _body;

	public LoggedParsedIdentityTextHost(IStopAware<Guid> operation, ILoggerFactory factory)
		: this(new LoggingAwareProcessor(operation, factory)) {}

	protected LoggedParsedIdentityTextHost(IStopAware<Guid> operation) : this(new ParsedIdentity(operation)) {}

	protected LoggedParsedIdentityTextHost(IAllocated<string> body) => _body = body;

	public Task Get(Stop<string> parameter) => _body.Get(parameter);
}