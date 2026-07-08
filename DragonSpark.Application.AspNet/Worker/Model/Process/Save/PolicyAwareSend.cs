using DragonSpark.Application.AspNet.Communication.Http.Diagnostics;
using DragonSpark.Model.Operations.Stop;
using Microsoft.Extensions.Logging;
using System;

namespace DragonSpark.Application.AspNet.Worker.Model.Process.Save;

public class PolicyAwareSend : PolicyAwareClientRequest
{
	protected PolicyAwareSend(IStopAware<Guid> previous, ILogger logger, string template)
		: base(previous, logger, template) {}
}