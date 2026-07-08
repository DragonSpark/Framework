using DragonSpark.Model.Operations.Stop;
using System;

namespace DragonSpark.Application.AspNet.Worker.Model;

public class Send : StopAware<Guid>
{
	protected Send(IStopAware<Guid> send) : base(send) {}
}