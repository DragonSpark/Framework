using DragonSpark.Application.AspNet.Communication.Http.Diagnostics;
using DragonSpark.Model.Operations.Stop;
using Microsoft.Extensions.Logging;

namespace DragonSpark.Application.AspNet.Workers.Model.Save;

public class PolicyAwareSend : PolicyAwareClientRequest
{
	protected PolicyAwareSend(IStopAware<Guid> previous, ILogger logger, string template)
		: base(previous, logger, template) {}
}