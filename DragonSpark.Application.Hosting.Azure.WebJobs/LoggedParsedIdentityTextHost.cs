using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using DragonSpark.Model.Operations.Stop;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public class LoggedParsedIdentityTextHost : IAllocatedStopAware<string>
{
	readonly IAllocatedStopAware<string> _body;

	public LoggedParsedIdentityTextHost(IStopAware<Guid> operation, ILoggerFactory factory)
		: this(new LoggingAwareProcessor(operation, factory)) {}

	protected LoggedParsedIdentityTextHost(IStopAware<Guid> operation) : this(new ParsedIdentity(operation)) {}

	protected LoggedParsedIdentityTextHost(IAllocatedStopAware<string> body) => _body = body;

	public Task Get(Stop<string> parameter) => _body.Get(parameter);
}