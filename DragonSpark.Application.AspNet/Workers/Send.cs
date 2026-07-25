using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.AspNet.Workers;

public class Send : StopAware<Guid>
{
	protected Send(IStopAware<Guid> send) : base(send) {}
}