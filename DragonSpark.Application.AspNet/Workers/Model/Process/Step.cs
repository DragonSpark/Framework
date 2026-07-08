using DragonSpark.Application.AspNet.Workers.Processes;
using DragonSpark.Model.Operations.Stop;
using System;

namespace DragonSpark.Application.AspNet.Workers.Model.Process;

public record Step<T>(IStopAware<T> Body, string Message, Guid Identifier) where T : ExternalProcess
{
	protected Step(IStopAware<T> Body, string Message, Type type) : this(Body, Message, type.GUID) {}
}