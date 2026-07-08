using DragonSpark.Application.AspNet.Worker.Processes;
using DragonSpark.Model.Operations.Stop;
using System;

namespace DragonSpark.Application.AspNet.Worker.Model.Process;

public record Step<T>(IStopAware<T> Body, string Message, Guid Identifier) where T : ExternalProcess
{
	protected Step(IStopAware<T> Body, string Message, Type type) : this(Body, Message, type.GUID) {}
}