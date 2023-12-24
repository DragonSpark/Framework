using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Allocated;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Hosting.Azure.WebJobs;

public class LoggedParsedIdentityTextHost : IAllocated<string>
{
	readonly IAllocated<string> _body;

	protected LoggedParsedIdentityTextHost(IOperation<Guid> operation, ILoggerFactory factory)
		: this(new LoggingAwareProcessor(operation, factory)) {}

	protected LoggedParsedIdentityTextHost(IOperation<Guid> operation) : this(new ParsedIdentity(operation)) {}

	protected LoggedParsedIdentityTextHost(IAllocated<string> body) => _body = body;

	public virtual Task Get(string parameter) => _body.Get(parameter);
}